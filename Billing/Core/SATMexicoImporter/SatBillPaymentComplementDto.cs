/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing SATMexico Importer                 Component : Interface adapters                      *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Output DTO                              *
*  Type     : SATBillDto                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return a SAT Mexico bill payment complement object.                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Billing.SATMexicoImporter {

  /// <summary>Output DTO used to return a SAT Mexico bill payment complement object.</summary>
  public class SatBillPaymentComplementDto : ISATBillDto {

    public SATBillGeneralDataDto DatosGenerales {
      get; internal set;
    } = new SATBillGeneralDataDto();


    public SATBillOrganizationDto Emisor {
      get; internal set;
    } = new SATBillOrganizationDto();


    public SATBillOrganizationDto Receptor {
      get; internal set;
    } = new SATBillOrganizationDto();


    public FixedList<SATBillConceptDto> Conceptos {
      get; internal set;
    } = new FixedList<SATBillConceptDto>();


    public PaymentComplementDataDto DatosComplemento {
      get; internal set;
    } = new PaymentComplementDataDto();


    public SATBillComplementDto SATComplemento {
      get; internal set;
    } = new SATBillComplementDto();

  } // class SatBillPaymentComplementDto


  public class PaymentComplementDataDto {

    public string PagosVersion {
      get; internal set;
    }


    public FixedList<ComplementBalanceDataDto> SaldosTotales {
      get; internal set;
    } = new FixedList<ComplementBalanceDataDto>();


    public FixedList<ComplementRelatedPayoutDataDto> DatosComplementoPago {
      get; internal set;
    } = new FixedList<ComplementRelatedPayoutDataDto>();

  }


  public class ComplementBalanceDataDto {

    public decimal TotalTrasladosBaseIVA16 {
      get; internal set;
    }


    public decimal TotalTrasladosImpuestoIVA16 {
      get; internal set;
    }


    public decimal MontoTotalPagos {
      get; internal set;
    }

  }


  public class ComplementRelatedPayoutDataDto {

    public DateTime FechaPago {
      get; internal set;
    }


    public string FormaDePagoP {
      get; internal set;
    }


    public string MonedaP {
      get; internal set;
    }


    public string TipoCambioP {
      get; internal set;
    }


    public decimal Monto {
      get; internal set;
    }


    public string NumOperacion {
      get; internal set;
    }


    public FixedList<ComplementRelatedDocumentDataDto> RelatedDocumentData {
      get; internal set;
    } = new FixedList<ComplementRelatedDocumentDataDto>();

  }


  public class ComplementRelatedDocumentDataDto {

    public string IdDocumento {
      get; internal set;
    }


    public string MonedaDR {
      get; internal set;
    }


    public string EquivalenciaDR {
      get; internal set;
    }


    public string NumParcialidad {
      get; internal set;
    }


    public decimal ImpSaldoAnt {
      get; internal set;
    }


    public decimal ImpPagado {
      get; internal set;
    }


    public decimal ImpSaldoInsoluto {
      get; internal set;
    }


    public string ObjetoImpDR {
      get; internal set;
    }


    public FixedList<SATBillTaxDto> Taxes {
      get; internal set;
    } = new FixedList<SATBillTaxDto>();

  }


} // namespace Empiria.Billing.SATMexicoImporter
