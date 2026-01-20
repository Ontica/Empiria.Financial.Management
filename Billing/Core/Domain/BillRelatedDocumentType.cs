/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Interface adapters                      *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Enumeration                             *
*  Type     : BillRelatedDocumentType                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Enumerates a bill tax factor type.                                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Billing {

  public enum BillRelatedDocumentType {

    NotaDeCredito = 01,

    NotaDeDebito = 02,

    DevolucionDeMercancia = 03,

    SustituciónDeCFDIPrevios = 04,

    TrasladoDeMercanciasFacturadasPreviamente = 05,

    FacturaGeneradaPorTrasladosPrevios = 06,

    AplicacionDeAnticipo = 07,

    Ninguno = 00

  } // enum BillRelatedDocumentType


  /// <summary>Extension methods for BillRelatedDocumentType.</summary>
  static public class BillRelatedDocumentTypeExtensions {

    static public NamedEntityDto MapToDto(this BillRelatedDocumentType relatedDocType) {
      return new NamedEntityDto(relatedDocType.ToString(), relatedDocType.ToString());
    }

  } // class BillRelatedDocumentTypeExtensions

} // namespace Empiria.Billing
