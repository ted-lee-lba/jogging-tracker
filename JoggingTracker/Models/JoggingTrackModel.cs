using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JoggingTracker;

namespace JoggingTracker.Models {
    public class JoggingTrackModel {
        public int JoggingTrack_Id { get; set; }
        public int Users_Id { get; set; }
        public System.DateTime FromDateTime { get; set; }
        public System.DateTime ToDateTime { get; set; }
        public decimal Distance { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTime UpdatedDate { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public double AverageSpeed {
            get {
                var totalInMinute = this.TimeUsed.TotalMinutes;
                return (double)this.Distance / totalInMinute;
            }
        }

        public int YearWeekNumber {
            get {
                return this.FromDateTime.GetWeekNumber();
            }
        }

        public TimeSpan TimeUsed {
            get {
                return ToDateTime.Subtract(FromDateTime);
            }
        }

    }
}