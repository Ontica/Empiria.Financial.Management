/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Interface adapters                      *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Enumeration                             *
*  Type     : BillTaxFactorType                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Enumerates a bill tax factor type.                                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Billing {

  /// <summary>Enumerates a bill tax factor type.</summary>
  public enum BillTaxFactorType {

    Cuota = 'C',

    Tasa = 'T',

    Exento = 'E',

    None = 'X'

  }  // enum BillTaxFactorType


  /// <summary>Extension methods for BillTaxFactorType.</summary>
  static public class BillTaxFactorTypeExtensions {

    static public NamedEntityDto MapToDto(this BillTaxFactorType status) {
      return new NamedEntityDto(status.ToString(), status.ToString());
    }

  }  // class BillTaxFactorTypeExtensions

} // namespace Empiria.Billing
