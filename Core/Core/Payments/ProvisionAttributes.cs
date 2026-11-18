/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Json attributes structure               *
*  Type     : ProvisionAttributes                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Defines a provision attributes structure. Provision attributes are used to represent the       *
*             state and metadata of a payment provision in the financial system.                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;
using Empiria.Parties;

namespace Empiria.Financial {

  /// <summary>Defines a provision attributes structure. Provision attributes are used to represent the
  /// state and metadata of a payment provision in the financial system.</summary>
  public class ProvisionAttributes {

    #region Constructors and Parsers

    private JsonObject _json = new JsonObject();

    public ProvisionAttributes() {
      // Required by Empiria Framework.
    }

    static public ProvisionAttributes Parse(JsonObject provisionData) {
      Assertion.Require(provisionData, nameof(provisionData));

      return new ProvisionAttributes() {
        _json = provisionData
      };
    }

    #endregion Constructors and Parsers

    #region Properties

    public bool CanClose {
      get {
        return Status == ProvisionStatus.Requested;
      }
    }


    public bool CanCancel {
      get {
        return CanClose;
      }
    }


    public bool CanRequest {
      get {
        return Status != ProvisionStatus.Requested && Status != ProvisionStatus.Closed;
      }
    }


    public Party RequestedBy {
      get {
        return _json.Get("requestedBy", Party.Empty);
      }
      private set {
        _json.SetIf("requestedBy", value.Id, !value.IsEmptyInstance);
      }
    }


    public DateTime RequestedTime {
      get {
        return _json.Get("requestedTime", DateTime.MinValue);
      }
      private set {
        _json.SetIf("requestedTime", value, value != DateTime.MinValue);
      }
    }


    public ProvisionStatus Status {
      get {
        return _json.Get("status", ProvisionStatus.None);
      }
      private set {
        _json.SetIf("status", value, value != ProvisionStatus.None);
      }
    }

    #endregion Properties

    #region Methods

    public void Close() {
      Assertion.Require(CanClose, "Cannot close a provision that has already been closed or canceled.");

      Status = ProvisionStatus.Closed;

      SetTag();
    }


    public void Cancel() {
      Assertion.Require(CanCancel, "Cannot cancel a provision that is not requested.");

      Status = ProvisionStatus.Canceled;

      SetTag();
    }


    public void Request() {
      Assertion.Require(CanRequest, "Cannot request a provision that has already been requested.");

      RequestedTime = DateTime.Now;
      RequestedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
      Status = ProvisionStatus.Requested;

      SetTag();
    }


    private void SetTag() {
      string tag = Status.GetTag();

      if (tag.Length == 0) {
        _json.Remove("tag");

      } else {
        _json.Set("tag", tag);
      }
    }

    #endregion Methods

  }  // class ProvisionAttributes

}  // namespace Empiria.Financial
