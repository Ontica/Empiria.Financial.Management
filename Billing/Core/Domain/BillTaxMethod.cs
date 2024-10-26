/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Interface adapters                      *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Enumeration                             *
*  Type     : BillTaxMethod                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Enumerates a bill tax application method according to SAT Mexico fiscal rules.                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Billing {

  /// <summary>Enumerates a bill tax application method according to SAT Mexico fiscal rules.</summary>
  public enum BillTaxMethod {

    Traslado = 'T',

    Retencion = 'R'

  }  // enum BillTaxMethod

} // namespace Empiria.Billing
