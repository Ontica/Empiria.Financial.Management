/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing SATMexico Importer                 Component : Interface adapters                      *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Output DTO                              *
*  Type     : SATBillDto                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return a SAT Mexico bill object.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Billing.SATMexicoImporter {


  public interface ISATBillDto {

  }


  /// <summary>Output DTO used to return a SAT Mexico bill object.</summary>
  public class SATBillDto : ISATBillDto {

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


    public SATBillComplementDto SATComplemento {
      get; internal set;
    } = new SATBillComplementDto();


    public FixedList<BillComplementFiscalLegend> LeyendasFiscales {
      get; internal set;
    } = new FixedList<BillComplementFiscalLegend>();


    public FixedList<BillComplementLocalTax> ImpuestosLocales {
      get; internal set;
    } = new FixedList<BillComplementLocalTax>();
    

    public SATBillAddenda Addenda {
      get; internal set;
    }


  } // class SATBillDto


  public class SATBillConceptDto {

    public string ClaveProdServ {
      get; internal set;
    }


    public string ClaveUnidad {
      get; internal set;
    }


    public decimal Cantidad {
      get; internal set;
    }


    public string Unidad {
      get; internal set;
    }


    public string NoIdentificacion {
      get; internal set;
    }


    public string Descripcion {
      get; internal set;
    }


    public decimal ValorUnitario {
      get; internal set;
    }


    public decimal Importe {
      get; internal set;
    }


    public decimal Descuento {
      get; internal set;
    }


    public string ObjetoImp {
      get; internal set;
    }


    public FixedList<SATBillTaxDto> Impuestos {
      get; internal set;
    } = new FixedList<SATBillTaxDto>();


    public bool IsBonusDiscount {
      get; internal set;
    }

  }  // class SATBillConceptDto


  public class SATBillGeneralDataDto {

    public string CFDIVersion {
      get; internal set;
    }


    public string Folio {
      get; internal set;
    }


    public DateTime Fecha {
      get; internal set;
    }


    public string Sello {
      get; internal set;
    }


    public string Serie {
      get; internal set;
    }


    public string NoCertificado {
      get; internal set;
    }


    public string Certificado {
      get; internal set;
    }


    public string FormaPago {
      get; internal set;
    }


    public string MetodoPago {
      get; internal set;
    }


    public string CondicionesPago {
      get; internal set;
    }


    public string Moneda {
      get; internal set;
    }


    public string TipoCambio {
      get; internal set;
    }


    public string TipoDeComprobante {
      get; internal set;
    }


    public string Exportacion {
      get; internal set;
    }


    public string LugarExpedicion {
      get; internal set;
    }


    public decimal SubTotal {
      get; internal set;
    }


    public decimal Descuento {
      get; internal set;
    }


    public decimal Total {
      get; internal set;
    }


    public FixedList<SATBillCFDIRelatedDataDto> CfdiRelacionados {
      get; internal set;
    } = new FixedList<SATBillCFDIRelatedDataDto>();

  }  // class SATBillGeneralDataDto


  public class SATBillOrganizationDto {

    public string RegimenFiscal {
      get; internal set;
    }


    public string RFC {
      get; internal set;
    }


    public string Nombre {
      get; internal set;
    }


    public string DomicilioFiscal {
      get; internal set;
    } = string.Empty;


    public string UsoCFDI {
      get; internal set;
    } = string.Empty;

  }  // class SATBillOrganizationDto


  public class SATBillCFDIRelatedDataDto {

    public string TipoRelacion {
      get; internal set;
    } = string.Empty;


    public string UUID {
      get; internal set;
    } = string.Empty;

  } // class SATBillRelatedDataDto


  public class SATBillTaxDto {

    public BillTaxMethod MetodoAplicacion {
      get; internal set;
    }


    public decimal Base {
      get; internal set;
    }


    public string Impuesto {
      get; internal set;
    }


    public string TipoFactor {
      get; internal set;
    }


    public decimal TasaOCuota {
      get; internal set;
    }


    public decimal Importe {
      get; internal set;
    }

  }  // class SATBillTaxDto


  public class BillComplementLocalTax {

    public BillTaxMethod MetodoAplicacion {
      get; internal set;
    }


    public string ImpLocalDescripcion {
      get; internal set;
    }


    public string Impuesto {
      get; internal set;
    }


    public string TipoFactor {
      get; internal set;
    }


    public decimal TasaDe {
      get; internal set;
    }


    public decimal CuotaDe {
      get; internal set;
    }


    public decimal Importe {
      get; internal set;
    }

  } // class BillComplementLocalTax


  /// <summary>BillComplementFiscalLegend</summary>
  public class BillComplementFiscalLegend {

    public string DisposicionFiscal {
      get; internal set;
    }


    public string Norma {
      get; internal set;
    }


    public string TextoLeyenda {
      get; internal set;
    }

  } // BillComplementFiscalLegend


  public class SATBillComplementDto {


    public string Xmlns_Tfd {
      get; internal set;
    }


    public string Xmlns_Xsi {
      get; internal set;
    }


    public string Xsi_SchemaLocation {
      get; internal set;
    }


    public string Tfd_Version {
      get; internal set;
    }


    public string UUID {
      get; internal set;
    }


    public DateTime FechaTimbrado {
      get; internal set;
    }


    public string RfcProvCertif {
      get; internal set;
    }


    public string SelloCFD {
      get; internal set;
    }


    public string NoCertificadoSAT {
      get; internal set;
    }


    public string SelloSAT {
      get; internal set;
    }

  }  // class SATBillComplementDto


  public class SATBillAddenda {

    public string NoEstacion {
      get; internal set;
    } = string.Empty;


    public string ClavePemex {
      get; internal set;
    } = string.Empty;


    public string Serie {
      get; internal set;
    } = string.Empty;


    public string Folio {
      get; internal set;
    } = string.Empty;


    public DateTime FechaEmision {
      get; internal set;
    }


    public SATBillConceptDto Concepto {
      get; internal set;
    }


    public FixedList<SATBillAddendaConcept> EcoConcepts {
      get; internal set;
    } = new FixedList<SATBillAddendaConcept>();

  } // class SATBillAddenda


  public class SATBillAddendaConcept {

    public decimal TasaIEPS {
      get; internal set;
    }


    public decimal IEPS {
      get; internal set;
    }


    public decimal TasaIVA {
      get; internal set;
    }


    public decimal IVA {
      get; internal set;
    }


    public string NoIdentificacion {
      get; internal set;
    } = string.Empty;


    public decimal TasaAIEPS {
      get; internal set;
    }


    public decimal AIEPS {
      get; internal set;
    }

  }

} // namespace Empiria.Billing.SATMexicoImporter
