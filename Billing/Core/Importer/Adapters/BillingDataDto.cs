/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Billing                          Component : Interface adapters                      *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Data Transfer Object                    *
*  Type     : BillingDataDto                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return a Billing object.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Financial.Management.Billing.Adapters {


    public enum TaxType {

        Traslado,

        Retencion

    }


    /// <summary>Output DTO used to return a Billing object.</summary>
    public class BillingDto {


        public GeneralDataDto DatosGenerales {
            get; internal set;
        } = new GeneralDataDto();


        public OrganizationDto Emisor {
            get; internal set;
        } = new OrganizationDto();


        public OrganizationDto Receptor {
            get; internal set;
        } = new OrganizationDto();


        public FixedList<BillingConceptDto> Conceptos {
            get; internal set;
        } = new FixedList<BillingConceptDto>();


    } // class BillingDataDto


    public class GeneralDataDto {


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

    }


    public class OrganizationDto {


        public string RegimenFiscal {
            get; internal set;
        } = string.Empty;


        public string RFC {
            get; internal set;
        } = string.Empty;


        public string Nombre {
            get; internal set;
        } = string.Empty;


        public string DomicilioFiscal {
            get; internal set;
        } = string.Empty;


        public string UsoCFDI {
            get; internal set;
        } = string.Empty;

    }


    public class BillingConceptDto {


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


        public FixedList<BillingTaxDto> Impuestos {
            get; set;
        } = new FixedList<BillingTaxDto>();


    }


    public class BillingTaxDto {


        public TaxType TipoImpuesto {
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

    } // class BillingDto


} // namespace Empiria.Financial.Management.Billing.Adapters
