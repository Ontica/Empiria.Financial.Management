/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Interface adapters                      *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Output DTO                              *
*  Type     : BillDto                                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return a bill object.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Billing.Adapters {

  /// <summary>Output DTO used to return a bill object.</summary>
  public class BillDto {

    public BillGeneralDataDto DatosGenerales {
      get; internal set;
    } = new BillGeneralDataDto();


    public BillOrganizationDto Emisor {
      get; internal set;
    } = new BillOrganizationDto();


    public BillOrganizationDto Receptor {
      get; internal set;
    } = new BillOrganizationDto();


    public FixedList<BillConceptDto> Conceptos {
      get; internal set;
    } = new FixedList<BillConceptDto>();


  } // class BillingDataDto


  public class BillGeneralDataDto {


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

  }  // class BillGeneralDataDto


  public class BillOrganizationDto {

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

  }  // class BillOrganizationDto



  public class BillConceptDto {

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


    public FixedList<BillTaxDto> Impuestos {
      get; set;
    } = new FixedList<BillTaxDto>();

  }  // class BillConceptDto



  public class BillTaxDto {

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

  }  // class BillTaxDto

} // namespace Empiria.Billing.Adapters
