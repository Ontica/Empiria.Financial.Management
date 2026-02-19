/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Service provider                        *
*  Type     : PaymentsTimeWindow                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds information about the current payments time window.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;

namespace Empiria.Payments {

  public class PaymentsTimeWindow {

    private readonly StoredJson _storedJson;

    public const string FORMAT = @"hh\:mm";

    static private PaymentsTimeWindow _singleton;
    static private object _locker = new object();

    #region Constructors and parsers

    private PaymentsTimeWindow() {
      _storedJson = StoredJson.Parse("Payments.Time.Window");
    }


    static public PaymentsTimeWindow Instance {
      get {
        lock (_locker) {
          if (_singleton != null) {
            return _singleton;
          }
          lock (_locker) {
            if (_singleton == null) {
              _singleton = new PaymentsTimeWindow();
            }
          }
        }
        return _singleton;
      }
    }

    #endregion Constructors and parsers

    #region Properties

    public TimeSpan DEFAULT_START_TIME {
      get {
        var value = _storedJson.Value.Get<string>("defaultStartTime");

        return TimeSpan.Parse(value);
      }
    }


    public TimeSpan DEFAULT_END_TIME {
      get {
        var value = _storedJson.Value.Get<string>("defaultEndTime");

        return TimeSpan.Parse(value);
      }
    }


    public TimeSpan EndTime {
      get {
        if (UseDefaults) {
          return DEFAULT_END_TIME;
        }
        var value = _storedJson.Value.Get("endTime", DEFAULT_END_TIME.ToString(FORMAT));

        return TimeSpan.Parse(value);
      }
      private set {
        _storedJson.Value.SetIf("endTime", value.ToString(FORMAT), value != DEFAULT_END_TIME);
      }
    }


    public DateTime EndDateTime {
      get {
        if (UseDefaults) {
          return DateTime.Today.Date.Add(DEFAULT_END_TIME);
        } else {
          return DateTime.Today.Date.Add(EndTime);
        }
      }
    }


    public TimeSpan StartTime {
      get {
        if (UseDefaults) {
          return DEFAULT_START_TIME;
        }

        var value = _storedJson.Value.Get("startTime", DEFAULT_START_TIME.ToString(FORMAT));

        return TimeSpan.Parse(value);
      }
      private set {
        _storedJson.Value.SetIf("startTime", value.ToString(FORMAT), value != DEFAULT_START_TIME);
      }
    }


    public DateTime StartDateTime {
      get {
        if (UseDefaults) {
          return DateTime.Today.Date.Add(DEFAULT_START_TIME);
        } else {
          return DateTime.Today.Date.Add(StartTime);
        }
      }
    }


    private DateTime LastUpdate {
      get {
        return _storedJson.Value.Get("lastUpdate", DateTime.Now);
      }
      set {
        _storedJson.Value.Set("lastUpdate", value);
      }
    }


    private bool UseDefaults {
      get {
        return LastUpdate.Date != DateTime.Today;
      }
    }


    #endregion Properties

    #region Methods

    public bool IsInDefaultTimeWindow(DateTime date) {
      return DEFAULT_START_TIME <= date.TimeOfDay && date.TimeOfDay <= DEFAULT_END_TIME;
    }


    public bool IsInUrgentTimeWindow(DateTime date) {
      return StartTime <= date.TimeOfDay && date.TimeOfDay <= EndTime;
    }


    public void Update(TimeSpan startTime, TimeSpan endTime) {

      Assertion.Require(startTime.Days == 0, "Unrecognized start time.");
      Assertion.Require(endTime.Days == 0, "Unrecognized end time.");

      Assertion.Require(startTime < endTime, "startTime must be before the endTime.");

      StartTime = startTime;
      EndTime = endTime;

      LastUpdate = DateTime.Now;

      _storedJson.Save();
    }

    #endregion Methods

  }  //  class PaymentsTimeWindow

}  // namespace Empiria.Payments
