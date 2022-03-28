namespace HMSDigital.Common.ViewModels
{
    public class FHIRQueueRequest<T>
    {
        public string ResourceType { get { return typeof(T).FullName; } }
        public T Resource { get; set; }
        public int RequestType { get; set; }
    }
}
