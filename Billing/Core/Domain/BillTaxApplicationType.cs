/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Interface adapters                      *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Data Transfer Object                    *
*  Type     : BillTaxApplicationType                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Enumerates a bill tax application type according to SAT Mexico fiscal rules.                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Billing {

  /// <summary>Enumerates a bill tax application type according to SAT Mexico fiscal rules.</summary>
  public enum BillTaxApplicationType {

    Traslado,

    Retencion

  }  // enum BillTaxApplicationType

} // namespace Empiria.Billing
