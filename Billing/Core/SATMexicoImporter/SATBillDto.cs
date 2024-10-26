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

  /// <summary>Output DTO used to return a SAT Mexico bill object.</summary>
  public class SATBillDto {

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


  } // class SATBillDto



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


    public FixedList<SATBillTaxDto> Impuestos {
      get; set;
    } = new FixedList<SATBillTaxDto>();

  }  // class SATBillConceptDto



  public class SATBillTaxDto {

    public BillTaxApplicationType TipoAplicacionImpuesto {
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

} // namespace Empiria.Billing.SATMexicoImporter
