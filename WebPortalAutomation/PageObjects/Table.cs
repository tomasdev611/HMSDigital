using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

using System;
using System.Collections.Generic; // Dictionary type defined here

namespace WebPortalAutomation
{
    public partial class BasePage
    {
        public StaticTable GetTable(
            string elementHeaders,
            string elementRows,

            string parameterizedCategoryHeaders = null,
            IWebElement relativeToHeaders = null,

            string parameterizedCategoryRows = null,
            IWebElement relativeToRows = null,

            int retries = NumberOfRetries,
            int sleep = SleepThread,
            int waitElementTimeout = WaitLoadElementTimeout
        )
        {   
            Logger.Info($"Creating a table with headers --{elementHeaders}-- and rows --{elementRows}--");

            List<IWebElement> tableHeaders = FindManyWithRetries(
                element: elementHeaders,
                parameterizedCategory: parameterizedCategoryHeaders,
                relativeTo: relativeToHeaders,
                retries: retries,
                sleep: sleep,
                waitElementTimeout: waitElementTimeout
            );

            List<IWebElement> tableBodyRows = FindManyWithRetries(
                element: elementRows,
                parameterizedCategory: parameterizedCategoryRows,
                relativeTo: relativeToRows,
                retries: retries,
                sleep: sleep,
                waitElementTimeout: waitElementTimeout
            );

            List<List<IWebElement>> cells = new List<List<IWebElement>>();
            
            foreach(IWebElement row in tableBodyRows)
            {
                cells.Add(FindManyWithRetries(element: "Table Data", relativeTo: row));
            }

            StaticTable tableCreated = new StaticTable(tableHeaders, cells);

            Logger.Info($"Table with headers --{elementHeaders}-- and rows --{elementRows}-- created");
            return tableCreated;
        }
    }

    public class StaticTable
    {

        List<string> headers;
        List<StaticRow> body;

        public StaticTable(List<IWebElement> tableHeaders, List<List<IWebElement>> tableBodyCells)
        {
            this.headers = new List<string>();
            this.body = new List<StaticRow>();

            // Load all the headers
            foreach(IWebElement header in tableHeaders)
            {
                this.headers.Add(header.Text.Trim());
            }

            // Load all the rows
            for(int i = 0; i < tableBodyCells.Count; i++)
            {    
                this.body.Add(
                    new StaticRow(this.headers, tableBodyCells[i])
                );
            }
        }

        public string Cell(int row, string column)
        {
            if(row < 0 || body.Count <= row)
            {
                throw new Exception($"Row value must be between 0 and {body.Count - 1}");
            }

            return body[row][column];
        }

        public int NumberOfRows()
        {
            return this.body.Count;
        }

        public int NumberOfColumns()
        {
            return this.headers.Count;
        }

        public StaticRow this[int row]
        {
            get
            {
                if(row < 0 || body.Count <= row)
                {
                    throw new Exception($"Row value must be between 0 and {body.Count - 1}");
                }

                return body[row];
            }
        }
    }

    public class StaticRow
    {
        Dictionary<string, string> rowCells;

        public StaticRow(List<string> headers, List<IWebElement> cells)
        {
            // Length of headers MUST match length of cells in a row
            if(cells.Count != headers.Count)
            {
                throw new Exception("Invalid match between headers and rows");
            }

            // Load the content of every cell in a dict
            this.rowCells = new Dictionary<string, string>();

            // Iterate through cells, as the number of cells matches the number of headers then
            // the j-th cell will be in the column of the j-th header
            for(int j = 0; j < cells.Count; j++)
            {
                string cellContent = cells[j].Text.Trim();
                this.rowCells[headers[j]] = cellContent;
            }
        }

        public string this[string column]
        {
            get 
            {
                if(!rowCells.ContainsKey(column))
                {
                    throw new Exception($"Invalid column name: {column}");
                }
                return rowCells[column];
            }
        }
    }
}
