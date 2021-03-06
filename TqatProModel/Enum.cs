﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TqatProModel {
    enum ReportType {
        HISTORICAL,
        RUNNING,
        IDLING,
        GEOFENCE,
        ACC,
        OVERSPEED,
        TRACKERS,
        TRACKERS_GEOFENCE,
        //EXTERNAL_POWER,
        //URGENT,
        ALLCOMPANIES,
        ALLTRACKERS
    }
    public enum ExportFileType {
        TXT,
        CSV,
        PDF
    }

    public enum TextLogFileType {
        TXT,
        CSV
    }

    //public enum TextLogType {
    //    NONE,
    //    EVENT,
    //    TRACKER,
    //    EXCEPTION
    //}

    public enum ServerStatus {
        STOP,
        RUN
    }
}
