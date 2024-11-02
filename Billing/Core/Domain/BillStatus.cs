/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Enumeration                             *
*  Type     : BillStatus                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Enumerates the control status of a bill.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

namespace Empiria.Billing {

  /// <summary>Enumerates the control status of a bill.</summary>
  public enum BillStatus {

    Pending = 'P',

    Validated = 'V',

    Issued = 'I',

    Payed = 'Y',

    Canceled = 'N',

    Deleted = 'X',

    All = '*',

  }  // enum BillStatus


  /// <summary>Extension methods for BillStatus.</summary>
  static public class BillStatusEnumExtensions {

    static public string GetName(this BillStatus status) {
      switch (status) {
        case BillStatus.Pending:
          return "Pendiente";
          
        case BillStatus.Validated:
          return "Validada";

        case BillStatus.Issued:
          return "Emitida";

        case BillStatus.Payed:
          return "Pagada";

        case BillStatus.Canceled:
          return "Cancelada";

        case BillStatus.Deleted:
          return "Eliminada";

        case BillStatus.All:
          return "Todas";

        default:
          throw Assertion.EnsureNoReachThisCode($"Unrecognized status {status}");
      }
    }


    static public NamedEntityDto MapToDto(this BillStatus status) {
      return new NamedEntityDto(status.ToString(), status.GetName());
    }

  }  // class BillStatusEnumExtensions


}  // namespace Empiria.Billing
