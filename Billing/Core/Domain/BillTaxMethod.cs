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

    Retencion = 'R',

    None = 'X'

  }  // enum BillTaxMethod


  /// <summary>Extension methods for BillTaxMethod.</summary>
  static public class BillTaxMethodExtensions {

    static public string GetName(this BillTaxMethod status) {
      switch (status) {
        case BillTaxMethod.Traslado:
          return "Traslado";

        case BillTaxMethod.Retencion:
          return "Retencion";

        case BillTaxMethod.None:
          return "Ninguno";

        default:
          throw Assertion.EnsureNoReachThisCode($"Unrecognized status {status}");
      }
    }


    static public NamedEntityDto MapToDto(this BillTaxMethod status) {
      return new NamedEntityDto(status.ToString(), status.GetName());
    }

  }  // class BillTaxMethodExtensions

} // namespace Empiria.Billing
