/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Enumeration                             *
*  Type     : BillStatus                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Enumerates the control status of a bill.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

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

}  // namespace Empiria.Billing
