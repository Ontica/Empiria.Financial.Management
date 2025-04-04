﻿/* Empiria Financial *****************************************************************************************
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


    public FixedList<SATBillConceptWithTaxDto> Conceptos {
      get; internal set;
    } = new FixedList<SATBillConceptWithTaxDto>();


    public SATBillComplementDto SATComplemento {
      get; internal set;
    } = new SATBillComplementDto();


    public SATBillAddenda Addenda {
      get; internal set;
    } = new SATBillAddenda();


  } // class SATBillDto


  public class SATBillConceptDto {

    public string ClaveProdServ {
      get; set;
    }


    public string ClaveUnidad {
      get; set;
    }


    public decimal Cantidad {
      get; set;
    }


    public string Unidad {
      get; set;
    }


    public string NoIdentificacion {
      get; set;
    }


    public string Descripcion {
      get; set;
    }


    public decimal ValorUnitario {
      get; set;
    }


    public decimal Importe {
      get; set;
    }


    public string ObjetoImp {
      get; set;
    }

  }  // class SATBillConceptDto


  public class SATBillConceptWithTaxDto : SATBillConceptDto {

    public FixedList<SATBillTaxDto> Impuestos {
      get; set;
    } = new FixedList<SATBillTaxDto>();

  } // SATBillConceptWithTaxDto


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


    public string Moneda {
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

    public string UUID {
      get; internal set;
    }

  } // class SATBillRelatedDataDto


  public class SATBillTaxDto {

    public BillTaxMethod MetodoAplicacion {
      get; set;
    }


    public decimal Base {
      get; set;
    }


    public string Impuesto {
      get; set;
    }


    public string TipoFactor {
      get; set;
    }


    public decimal TasaOCuota {
      get; set;
    }


    public decimal Importe {
      get; set;
    }

  }  // class SATBillTaxDto


  public class SATBillComplementDto {


    public string Xmlns_Tfd {
      get; set;
    }


    public string Xmlns_Xsi {
      get; set;
    }


    public string Xsi_SchemaLocation {
      get; set;
    }


    public string Tfd_Version {
      get; set;
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
      get; set;
    } = string.Empty;


    public string ClavePemex {
      get; set;
    } = string.Empty;


    public FixedList<SATBillAddendaConcept> EcoConcepts {
      get; set;
    } = new FixedList<SATBillAddendaConcept>();

  } // class SATBillAddenda


  public class SATBillAddendaConcept {

    public decimal TasaIEPS {
      get; set;
    }


    public decimal IEPS {
      get; set;
    }


    public decimal TasaIVA {
      get; set;
    }


    public decimal IVA {
      get; set;
    }


    public decimal NoIdentificacion {
      get; set;
    }


    public decimal TasaAIEPS {
      get; set;
    }


    public decimal AIEPS {
      get; set;
    }

  }

} // namespace Empiria.Billing.SATMexicoImporter
