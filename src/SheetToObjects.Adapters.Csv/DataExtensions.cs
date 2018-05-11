﻿using System.Collections.Generic;
using System.Linq;
using SheetToObjects.Lib;

namespace SheetToObjects.Adapters.Csv
{
    public static class DataExtensions
    {
        public static List<Row> ToRows(this List<List<string>> sheetData)
        {
            return sheetData.Select((rowData, rowIndex) => new Row(RowDataToCells(rowData, rowIndex), rowIndex)).ToList();
        }

        public static List<Cell> RowDataToCells(this List<string> rowData, int rowIndex)
        {
            return rowData.Select((cellData, columIndex) => new Cell(rowIndex+1, columIndex, cellData)).ToList();
        }
    }
}