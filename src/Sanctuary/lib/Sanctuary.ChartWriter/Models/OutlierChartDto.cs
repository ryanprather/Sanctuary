﻿namespace Sanctuary.ChartWriter.Models
{
    public class OutlierChartDto
    {
        public int OutlierDevation {  get; set; }
        public string PatientId { get; set; }
        public string ChartType { get; set; }
        public List<string> Labels { get; set; }
        public string DatasetLabel { get; set; }
        public List<decimal> Data {  get; set; }
    }
}
