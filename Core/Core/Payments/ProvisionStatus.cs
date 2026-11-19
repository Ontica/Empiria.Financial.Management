/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Enumeration Type                        *
*  Type     : ProvisionStatus                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Enumerates the status of a payment provision.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial {

  /// <summary>Enumerates the status of a payment provision.</summary>
  public enum ProvisionStatus {

    Candidate = 'N',

    Requested = 'R',

    Closed = 'C',

    Canceled = 'L',

    All = '@'

  }  // enum ProvisionStatus



  /// <summary>Provides extension methods for ProvisionStatus enumeration.</summary>
  static public class ProvisionStatusExtensions {

    static public string GetName(this ProvisionStatus status) {

      switch (status) {

        case ProvisionStatus.Candidate:
          return "Candidata";

        case ProvisionStatus.Requested:
          return "Solicitada";

        case ProvisionStatus.Closed:
          return "Provisionada";

        case ProvisionStatus.Canceled:
          return "Cancelada";

        case ProvisionStatus.All:
          return "Todas";

        default:
          throw Assertion.EnsureNoReachThisCode(
            $"Cannot get name for provision with status {status}."
          );
      }
    }


    static public string GetTag(this ProvisionStatus status) {

      switch (status) {

        case ProvisionStatus.Candidate:
        case ProvisionStatus.Canceled:
          return string.Empty;

        case ProvisionStatus.Requested:
          return "PROVISION_REQUESTED";

        case ProvisionStatus.Closed:
          return "PROVISION_CLOSED";

        default:
          throw Assertion.EnsureNoReachThisCode(
            $"Cannot get tag for provision with status {status}."
          );
      }

    }
  }  // class ProvisionStatusExtensions

}  //namespace Empiria.Financial
