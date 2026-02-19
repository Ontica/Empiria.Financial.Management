/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing SATMexico Importer                 Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Service provider                        *
*  Type     : SATBillXmlReader                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Service used to read from a bill as xml string and return general data to SATBillDto object.   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Presentation;

namespace Empiria.Billing.SATMexicoImporter {

  /// <summary>Service used to read from a bill as xml string and return
  /// general data to SATBillDto object</summary>
  internal class SATBillGeneralDataXmlReader {

    internal SATBillGeneralDataXmlReader() {

    }

    #region Public methods

    internal FixedList<SATBillConceptDto> GenerateConceptsList(XmlNode conceptsNode) {

      var conceptosDto = new List<SATBillConceptDto>();

      foreach (XmlNode concept in conceptsNode.ChildNodes) {

        if (!concept.Name.Equals("cfdi:Concepto") && !concept.Name.Equals("edr:Concepto")) {
          Assertion.EnsureFailed("The concepts node must contain only concepts.");
        }

        var conceptoDto = new SATBillConceptDto() {
          ClaveProdServ = GetAttribute(concept, "ClaveProdServ"),
          ClaveUnidad = GetAttribute(concept, "ClaveUnidad"),
          Cantidad = GetAttribute<decimal>(concept, "Cantidad"),
          Unidad = GetAttribute(concept, "Unidad"),
          NoIdentificacion = GetAttribute(concept, "NoIdentificacion"),
          Descripcion = GetAttribute(concept, "Descripcion"),
          ValorUnitario = GetAttribute<decimal>(concept, "ValorUnitario"),
          Importe = GetAttribute<decimal>(concept, "Importe"),
          Descuento = GetAttribute<decimal>(concept, "Descuento"),
          ObjetoImp = GetAttribute(concept, "ObjetoImp"),
          Impuestos = GenerateTaxesByConcept(concept.ChildNodes)
        };

        conceptosDto.Add(conceptoDto);

      }
      return conceptosDto.ToFixedList();

    }


    internal FixedList<SATBillCFDIRelatedDataDto> GenerateRelatedCfdiData(XmlNode cfdiRelatedNode) {

      var cfdiRelatedListDto = new List<SATBillCFDIRelatedDataDto>();

      foreach (XmlNode cfdiRelated in cfdiRelatedNode.ChildNodes) {

        if (!cfdiRelated.Name.Equals("cfdi:CfdiRelacionado")) {
          Assertion.EnsureFailed("The node name must be cfdi:CfdiRelacionado.");
        }

        var cfdiRelatedDto = new SATBillCFDIRelatedDataDto() {
          TipoRelacion = GetAttribute(cfdiRelatedNode, "TipoRelacion"),
          UUID = GetAttribute(cfdiRelated, "UUID"),
        };

        cfdiRelatedListDto.Add(cfdiRelatedDto);
      }
      return cfdiRelatedListDto.ToFixedList();
    }


    internal SATBillGeneralDataDto GenerateGeneralData(XmlElement generalDataNode) {

      if (!generalDataNode.Name.Equals("cfdi:Comprobante")) {
        Assertion.EnsureFailed("The xml file is not a valid CFDI document.");
      } else if (!generalDataNode.GetAttribute("Version").Equals("4.0")) {
        Assertion.EnsureFailed("The CFDI version is not correct.");
      }

      return new SATBillGeneralDataDto {
        CFDIVersion = GetAttribute(generalDataNode, "Version"),
        Folio = GetAttribute(generalDataNode, "Folio"),
        Fecha = GetAttribute<DateTime>(generalDataNode, "Fecha"),
        Sello = GetAttribute(generalDataNode, "Sello"),
        Serie = GetAttribute(generalDataNode, "Serie"),
        FormaPago = GetAttribute(generalDataNode, "FormaPago"),
        CondicionesPago = GetAttribute(generalDataNode, "CondicionesDePago"),
        NoCertificado = GetAttribute(generalDataNode, "NoCertificado"),
        Certificado = GetAttribute(generalDataNode, "Certificado"),
        Moneda = GetAttribute(generalDataNode, "Moneda"),
        SubTotal = GetAttribute<decimal>(generalDataNode, "SubTotal"),
        Descuento = GetAttribute<decimal>(generalDataNode, "Descuento"),
        Total = GetAttribute<decimal>(generalDataNode, "Total"),

        TipoCambio = GetAttribute(generalDataNode, "TipoCambio"),
        TipoDeComprobante = GetAttribute(generalDataNode, "TipoDeComprobante"),
        Exportacion = GetAttribute(generalDataNode, "Exportacion"),
        MetodoPago = GetAttribute(generalDataNode, "MetodoPago"),
        LugarExpedicion = GetAttribute(generalDataNode, "LugarExpedicion"),
      };

    }


    internal SATBillOrganizationDto GenerateReceiverData(XmlNode receiverNode) {

      return new SATBillOrganizationDto {
        RegimenFiscal = GetAttribute(receiverNode, "RegimenFiscalReceptor"),
        RFC = GetAttribute(receiverNode, "Rfc"),
        Nombre = GetAttribute(receiverNode, "Nombre"),
        DomicilioFiscal = GetAttribute(receiverNode, "DomicilioFiscalReceptor"),
        UsoCFDI = GetAttribute(receiverNode, "UsoCFDI"),
      };
    }


    internal SATBillComplementDto GenerateSATComplementData(XmlNode timbre) {

      return new SATBillComplementDto {
        Xmlns_Tfd = GetAttribute(timbre, "xmlns:tfd"),
        Xmlns_Xsi = GetAttribute(timbre, "xmlns:xsi"),
        Xsi_SchemaLocation = GetAttribute(timbre, "xsi:schemaLocation"),
        Tfd_Version = GetAttribute(timbre, "Version"),
        UUID = GetAttribute(timbre, "UUID"),
        FechaTimbrado = GetAttribute<DateTime>(timbre, "FechaTimbrado"),
        RfcProvCertif = GetAttribute(timbre, "RfcProvCertif"),
        SelloCFD = GetAttribute(timbre, "SelloCFD"),
        NoCertificadoSAT = GetAttribute(timbre, "NoCertificadoSAT"),
        SelloSAT = GetAttribute(timbre, "SelloSAT")
      };
    }


    internal SATBillOrganizationDto GenerateSenderData(XmlNode senderNode) {

      return new SATBillOrganizationDto {
        RegimenFiscal = GetAttribute(senderNode, "RegimenFiscal"),
        RFC = GetAttribute(senderNode, "Rfc"),
        Nombre = GetAttribute(senderNode, "Nombre"),
      };
    }


    internal FixedList<BillComplementLocalTax> GenerateImpuestosLocales(XmlNode impuestosLocalesNode) {

      var impuestosLocales = new List<BillComplementLocalTax>();

      foreach (XmlNode impuestoLocal in impuestosLocalesNode.ChildNodes) {

        BillComplementLocalTax impuesto = new BillComplementLocalTax();
        
        if (impuestoLocal.Name.Equals("implocal:TrasladosLocales")) {

          impuesto.ImpLocalDescripcion = GetAttribute(impuestoLocal, "ImpLocTrasladado");
          impuesto.TasaDe = GetAttribute<decimal>(impuestoLocal, "TasadeTraslado");
          impuesto.CuotaDe = GetAttribute<decimal>(impuestoLocal, "CuotadeTraslado");

        } else if (impuestoLocal.Name.Equals("implocal:RetencionesLocales")) {

          impuesto.ImpLocalDescripcion = GetAttribute(impuestoLocal, "ImpLocRetenido");
          impuesto.TasaDe = GetAttribute<decimal>(impuestoLocal, "TasadeRetencion");
          impuesto.CuotaDe = GetAttribute<decimal>(impuestoLocal, "CuotadeRetencion");

        } else {
          throw Assertion.EnsureNoReachThisCode($"Unhandled Xml node SAT tax method: {impuestoLocal.Name}");
        }

        impuesto.MetodoAplicacion = AssignBillTaxMethod(impuestoLocal);
        impuesto.Impuesto = "Impuesto local";
        impuesto.Importe = GetAttribute<decimal>(impuestoLocal, "Importe");
        impuesto.TipoFactor = GetFactorTypeForComplementLocalTax(impuesto);
        impuestosLocales.Add(impuesto);
      }
      return new FixedList<BillComplementLocalTax>(impuestosLocales);
    }


    internal FixedList<BillComplementFiscalLegend> GenerateLeyendasFiscales(XmlNode leyendasFisc) {

      var leyendas = new List<BillComplementFiscalLegend>();

      foreach (XmlNode leyendaFisc in leyendasFisc.ChildNodes) {

        if (leyendaFisc.Name.Equals("leyendasFisc:Leyenda")) {

          BillComplementFiscalLegend leyenda = new BillComplementFiscalLegend {
            DisposicionFiscal = GetAttribute(leyendaFisc, "disposicionFiscal"),
            Norma = GetAttribute(leyendaFisc, "norma"),
            TextoLeyenda = GetAttribute(leyendaFisc, "textoLeyenda")
          };
          leyendas.Add(leyenda);
        }
      }
      return new FixedList<BillComplementFiscalLegend>(leyendas);
    }



    internal string GetAttribute(XmlNode concept, string attributeName) {
      return Patcher.Patch(concept.Attributes[attributeName]?.Value, string.Empty);
    }


    internal T GetAttribute<T>(XmlNode concept, string attributeName) {
      if (concept.Attributes[attributeName]?.Value == null) {
        return default;
      } else {
        return (T) Convert.ChangeType(concept.Attributes[attributeName]?.Value, typeof(T));
      }
    }


    internal SATBillTaxDto GetTaxItem(XmlNode taxItem) {

      string tipoFactor = GetAttribute(taxItem, "TipoFactor") == string.Empty ? "None" :
                          GetAttribute(taxItem, "TipoFactor");
      
      return new SATBillTaxDto {
        Base = GetAttribute<decimal>(taxItem, "Base"),
        Impuesto = GetAttribute(taxItem, "Impuesto"),
        TipoFactor = tipoFactor,
        TasaOCuota = GetAttribute<decimal>(taxItem, "TasaOCuota"),
        Importe = GetAttribute<decimal>(taxItem, "Importe"),
        MetodoAplicacion = AssignBillTaxMethod(taxItem)
      };
    }


    internal IEnumerable<SATBillTaxDto> GetTaxItems(XmlNode taxNode) {
      var taxesByConceptDto = new List<SATBillTaxDto>();

      foreach (XmlNode taxItem in taxNode.ChildNodes) {

        taxesByConceptDto.Add(GetTaxItem(taxItem));
      }

      return taxesByConceptDto;
    }

    #endregion Public methods


    #region Services

    private BillTaxMethod AssignBillTaxMethod(XmlNode taxItem) {

      if (taxItem.Name.Contains(":Traslado")) {
        return BillTaxMethod.Traslado;

      } else if (taxItem.Name.Contains(":Retencion")) {
        return BillTaxMethod.Retencion;

      } else {

        throw Assertion.EnsureNoReachThisCode($"Unhandled SAT tax type: {taxItem.Name}");
      }
    }


    private FixedList<SATBillTaxDto> GenerateTaxesByConcept(XmlNodeList childNodes) {

      var taxesByConceptDto = new List<SATBillTaxDto>();

      foreach (XmlNode taxesNode in childNodes) {

        if (taxesNode.Name.Equals("cfdi:Impuestos") || taxesNode.Name.Equals("edr:Impuestos")) {

          foreach (XmlNode taxNode in taxesNode.ChildNodes) {

            taxesByConceptDto.AddRange(GetTaxItems(taxNode));
          }
        }
      }
      return taxesByConceptDto.ToFixedList();
    }


    private string GetFactorTypeForComplementLocalTax(BillComplementLocalTax impuesto) {

      string tipoFactor = string.Empty;

      if (impuesto.TasaDe > 0 && impuesto.CuotaDe == 0) {
        tipoFactor = "Tasa";

      } else if (impuesto.TasaDe == 0 && impuesto.CuotaDe > 0) {
        tipoFactor = "Cuota";

      } else {

        tipoFactor = "None";
      }
      return tipoFactor;
    }

    #endregion Services

  } // class SATBillGeneralDataXmlReader

} // namespace Empiria.Billing.SATMexicoImporter
