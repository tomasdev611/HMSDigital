using HMSDigital.EHRIntegration.HCHB.MLLPServer.Config;
using HMSDigital.EHRIntegration.HCHB.MLLPServer.Contracts;
using HMSDigital.EHRIntegration.HCHB.MLLPServer.DTOs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HMSDigital.EHRIntegration.HCHB.MLLPServer
{
    public class MLLPServer : BackgroundService
    {
        private static char START_OF_BLOCK = (char)0x0B;
        private static char END_OF_BLOCK = (char)0x1C;
        private static char CARRIAGE_RETURN = (char)13;
        private static int MESSAGE_CONTROL_ID_LOCATION = 9;
        private static char FIELD_DELIMITER = '|';

        private TcpListener _tcpListener;
        private readonly ILogger<MLLPServer> _logger;

        private readonly IAzureServiceBusClient _asbClient;
        private readonly MLLPConfig _mllpConfig;

        public MLLPServer(ILogger<MLLPServer> logger, IAzureServiceBusClient asbClient, IOptions<MLLPConfig> mllpConfig)
        {
            _logger = logger;
            _asbClient = asbClient;
            _mllpConfig = mllpConfig.Value;
        } 

        private Task StartTcpServer(int portNumberToListenOn)
        {
            _tcpListener = new TcpListener(IPAddress.Any, portNumberToListenOn);

            _logger.LogInformation("TCP Server started at: {time}", DateTimeOffset.Now);

            _tcpListener.Start();

            return Task.CompletedTask;
        }

        private void AcceptClientConnection() 
        {       
            try 
            {                
                //wait for client connections to come in
                var incomingTcpClientConnection = _tcpListener.AcceptTcpClient();

                _logger.LogInformation("Incoming TCP Client connection accepted at: {time}", DateTimeOffset.Now);

                //create a new thread to process this client connection
                var clientProcessingThread = new Thread(ProcessClientConnection);

                //start processing client connections to this server
                clientProcessingThread.Start(incomingTcpClientConnection);                               
            }
            catch (Exception ex)
            {
                //print any exceptions during the communications to the console
                Console.WriteLine(ex.Message);               
            }            
        }

        private void ProcessClientConnection(object forThreadProcessing)
        {            
            //the argument passed to the thread delegate is the incoming tcp client connection
            var tcpClientConnection = (TcpClient)forThreadProcessing;

            this._logger.LogInformation("Incoming TCP Client connection " + tcpClientConnection.Client.RemoteEndPoint + " processed at: {time}", DateTimeOffset.Now);            

            var receivedByteBuffer = new byte[200];
            var netStream = tcpClientConnection.GetStream();

            try
            {
                // Keep receiving data from the client closes connection
                int bytesReceived; // Received byte count
                var hl7Data = string.Empty;

                //keeping reading until there is data available from the client and echo it back
                while ((bytesReceived = netStream.Read(receivedByteBuffer, 0, receivedByteBuffer.Length)) > 0)
                {
                    hl7Data += Encoding.UTF8.GetString(receivedByteBuffer, 0, bytesReceived);

                    // Find start of MLLP frame, a VT character ...
                    var startOfMllpEnvelope = hl7Data.IndexOf(START_OF_BLOCK);
                    
                    if (startOfMllpEnvelope >= 0)
                    {
                        // Now look for the end of the frame, a FS character
                        var end = hl7Data.IndexOf(END_OF_BLOCK);

                        if (end >= startOfMllpEnvelope) //end of block received
                        {
                            //if both start and end of block are recognized in the data transmitted, then extract the entire message
                            var hl7MessageData = hl7Data.Substring(startOfMllpEnvelope + 1, end - startOfMllpEnvelope);

                            //create a HL7 acknowledgement message
                            var ackMessage = CreateAcknowledgementMessage(hl7MessageData);

                            //echo the received data back to the client
                            var buffer = Encoding.UTF8.GetBytes(ackMessage);

                            if (netStream.CanWrite)
                            {
                                netStream.Write(buffer, 0, buffer.Length);

                                _logger.LogInformation("Ack message sent to TCP Client: {client}", tcpClientConnection.Client.RemoteEndPoint);
                            }

                            #region AzureServiceBus integration

                            //send the message through azure service bus
                            var adthl7MessageToSend = new HL7ADTDTO()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Date = DateTime.Now,
                                Message = hl7MessageData
                            };

                            _asbClient.AddMessageBatch<HL7ADTDTO>(new List<HL7ADTDTO>() { adthl7MessageToSend }).GetAwaiter();

                            #endregion
                        }
                    }

                }
            }
            catch (Exception e)
            {
                //log any exceptions during the communications
                _logger.LogError(e.InnerException + ": " + e.Message);
            }
            finally
            {
                // Close the stream and the connection with the client
                netStream.Close();
                netStream.Dispose();
                tcpClientConnection.Close();
            }
        }

        private string CreateAcknowledgementMessage(string incomingHl7Message)
        {
            if (string.IsNullOrEmpty(incomingHl7Message))
                throw new ApplicationException("Invalid HL7 message for parsing operation. Please check your inputs");

            //retrieve the message control ID of the incoming HL7 message
            var messageControlId = GetMessageControlID(incomingHl7Message);

            //build an acknowledgement message and include the control ID with it
            var ackMessage = new StringBuilder();
            ackMessage = ackMessage.Append(START_OF_BLOCK)
                .Append("MSH|^~\\&|||||||ACK||P|2.2")
                .Append(CARRIAGE_RETURN)
                .Append("MSA|AA|")
                .Append(messageControlId)
                .Append(CARRIAGE_RETURN)
                .Append(END_OF_BLOCK)
                .Append(CARRIAGE_RETURN);

            return ackMessage.ToString();
        }       

        private string GetMessageControlID(string incomingHl7Message)
        {

            var fieldCount = 0;
            //parse the message into segments using the end of segment separter
            var hl7MessageSegments = incomingHl7Message.Split(CARRIAGE_RETURN);

            //tokenize the MSH segment into fields using the field separator
            var hl7FieldsInMshSegment = hl7MessageSegments[0].Split(FIELD_DELIMITER);

            //retrieve the message control ID in order to reply back with the message ack
            foreach (var field in hl7FieldsInMshSegment)
            {
                if (fieldCount == MESSAGE_CONTROL_ID_LOCATION)                
                    return field;
                
                fieldCount++;
            }

            return string.Empty; //you can also throw an exception here if you wish
        }

        public void StopTCPServer()
        {
            //stop the TCP listener before you dispose of it
            _tcpListener?.Stop();

            _logger.LogInformation("TCP Server stopped at: {time}", DateTimeOffset.Now);
        }      
        

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await StartTcpServer(_mllpConfig.ServerPort);

            while (!stoppingToken.IsCancellationRequested)
            {
                AcceptClientConnection();
            }

            StopTCPServer();
        }
    }
}
