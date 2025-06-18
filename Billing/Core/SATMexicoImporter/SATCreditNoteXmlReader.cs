/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing SATMexico Importer                 Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Service provider                        *
*  Type     : SATCreditNoteXmlReader                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Service used to read a credit note as xml string and return a SATBillDto object.               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Xml;

namespace Empiria.Billing.SATMexicoImporter {

  /// <summary>Service used to read a credit note as xml string and return a SATBillDto object.</summary>
  internal class SATCreditNoteXmlReader {

    private readonly SATBillDto _satCreditNoteDto;

    private readonly XmlDocument _xmlDocument;

    internal SATCreditNoteXmlReader(string xmlString) {
      Assertion.Require(xmlString, nameof(xmlString));

      _satCreditNoteDto = new SATBillDto();

      _xmlDocument = new XmlDocument();

      _xmlDocument.LoadXml(xmlString);
    }

    #region Services

    internal SATBillDto ReadAsCreditNoteDto() {

      XmlElement generalData = _xmlDocument.DocumentElement;
      XmlNodeList nodes = generalData.ChildNodes;

      GenerateGeneralData(generalData);

      foreach (XmlNode node in nodes) {

        if (node.Name == "cfdi:CfdiRelacionados") {

          GenerateCfdiRelatedData(node);
        }
        if (node.Name == "cfdi:Emisor") {

          GenerateSenderData(node);
        }
        if (node.Name == "cfdi:Receptor") {

          GenerateReceiverData(node);
        }
        if (node.Name == "cfdi:Conceptos") {

          GenerateConceptsList(node);
        }
        if (node.Name == "cfdi:Complemento") {

          GenerateComplementData(node);
        }
      }

      return _satCreditNoteDto;
    }

    #endregion Services

    #region Helpers

    
    private void GenerateCfdiRelatedData(XmlNode cfdiRelatedNode) {

      var cfdiRelatedListDto = new List<SATBillCFDIRelatedDataDto>();

      foreach (XmlNode cfdiRelated in cfdiRelatedNode.ChildNodes) {

        if (!cfdiRelated.Name.Equals("cfdi:CfdiRelacionado")) {
          Assertion.EnsureFailed("The node name must be cfdi:CfdiRelacionado.");
        }

        var cfdiRelatedDto = new SATBillCFDIRelatedDataDto() {
          UUID = GetAttribute(cfdiRelated, "UUID"),
        };

        cfdiRelatedListDto.Add(cfdiRelatedDto);
      }
      _satCreditNoteDto.DatosGenerales.CfdiRelacionados = cfdiRelatedListDto.ToFixedList();
    }


    private void GenerateComplementData(XmlNode complementNode) {

      XmlNode timbre = complementNode.FirstChild;

      if (!timbre.Name.Equals("tfd:TimbreFiscalDigital"))
        Assertion.EnsureFailed("The 'tfd:TimbreFiscalDigital' it doesnt exist.");

      _satCreditNoteDto.SATComplemento = new SATBillComplementDto {
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


    private void GenerateConceptsList(XmlNode conceptsNode) {

      var conceptosDto = new List<SATBillConceptWithTaxDto>();

      foreach (XmlNode concept in conceptsNode.ChildNodes) {

        if (!concept.Name.Equals("cfdi:Concepto")) {
          Assertion.EnsureFailed("The concepts node must contain only concepts.");
        }

        var conceptoDto = new SATBillConceptWithTaxDto() {
          ClaveProdServ = GetAttribute(concept, "ClaveProdServ"),
          ClaveUnidad = GetAttribute(concept, "ClaveUnidad"),
          Cantidad = GetAttribute<decimal>(concept, "Cantidad"),
          Unidad = GetAttribute(concept, "Unidad"),
          NoIdentificacion = GetAttribute(concept, "NoIdentificacion"),
          Descripcion = GetAttribute(concept, "Descripcion"),
          ValorUnitario = GetAttribute<decimal>(concept, "ValorUnitario"),
          Importe = GetAttribute<decimal>(concept, "Importe"),
          ObjetoImp = GetAttribute(concept, "ObjetoImp"),
          Impuestos = GenerateTaxesByConcept(concept.ChildNodes.Item(0))
        };

        conceptosDto.Add(conceptoDto);

      }
      _satCreditNoteDto.Conceptos = conceptosDto.ToFixedList();
    }


    private void GenerateGeneralData(XmlElement generalDataNode) {

      if (!generalDataNode.Name.Equals("cfdi:Comprobante")) {
        Assertion.EnsureFailed("El archivo xml no es un cfdi.");
      } else if (!generalDataNode.GetAttribute("Version").Equals("4.0")) {
        Assertion.EnsureFailed("The CFDI version is not correct.");
      }

      _satCreditNoteDto.DatosGenerales = new SATBillGeneralDataDto {
        CFDIVersion = GetAttribute(generalDataNode, "Version"),
        Folio = GetAttribute(generalDataNode, "Folio"),
        Fecha = GetAttribute<DateTime>(generalDataNode, "Fecha"),
        Sello = GetAttribute(generalDataNode, "Sello"),
        Serie = GetAttribute(generalDataNode, "Serie"),
        FormaPago = GetAttribute(generalDataNode, "FormaPago"),
        NoCertificado = GetAttribute(generalDataNode, "NoCertificado"),
        Certificado = GetAttribute(generalDataNode, "Certificado"),
        SubTotal = GetAttribute<decimal>(generalDataNode, "SubTotal"),
        Moneda = GetAttribute(generalDataNode, "Moneda"),
        Total = GetAttribute<decimal>(generalDataNode, "Total"),

        TipoDeComprobante = GetAttribute(generalDataNode, "TipoDeComprobante"),
        Exportacion = GetAttribute(generalDataNode, "Exportacion"),
        MetodoPago = GetAttribute(generalDataNode, "MetodoPago"),
        LugarExpedicion = GetAttribute(generalDataNode, "LugarExpedicion"),
      };

    }


    private void GenerateReceiverData(XmlNode receiverNode) {

      _satCreditNoteDto.Receptor = new SATBillOrganizationDto {
        RegimenFiscal = GetAttribute(receiverNode, "RegimenFiscalReceptor"),
        RFC = GetAttribute(receiverNode, "Rfc"),
        Nombre = GetAttribute(receiverNode, "Nombre"),
        DomicilioFiscal = GetAttribute(receiverNode, "DomicilioFiscalReceptor"),
        UsoCFDI = GetAttribute(receiverNode, "UsoCFDI"),
      };
    }


    private void GenerateSenderData(XmlNode senderNode) {

      _satCreditNoteDto.Emisor = new SATBillOrganizationDto {
        RegimenFiscal = GetAttribute(senderNode, "RegimenFiscal"),
        RFC = GetAttribute(senderNode, "Rfc"),
        Nombre = GetAttribute(senderNode, "Nombre"),
      };
    }


    private FixedList<SATBillTaxDto> GenerateTaxesByConcept(XmlNode taxesNode) {

      var taxesByConceptDto = new List<SATBillTaxDto>();

      foreach (XmlNode taxNode in taxesNode.ChildNodes) {

        taxesByConceptDto.AddRange(GetTaxItems(taxNode));
      }
      return taxesByConceptDto.ToFixedList();
    }


    private string GetAttribute(XmlNode concept, string attributeName) {
      return Patcher.Patch(concept.Attributes[attributeName]?.Value, string.Empty);
    }


    private T GetAttribute<T>(XmlNode concept, string attributeName) {
      if (concept.Attributes[attributeName]?.Value == null) {
        return default;
      } else {
        return (T) Convert.ChangeType(concept.Attributes[attributeName]?.Value, typeof(T));
      }
    }


    private IEnumerable<SATBillTaxDto> GetTaxItems(XmlNode taxNode) {
      var taxesByConceptDto = new List<SATBillTaxDto>();

      foreach (XmlNode taxItem in taxNode.ChildNodes) {

        var taxDto = new SATBillTaxDto();

        taxDto.Base = Convert.ToDecimal(taxItem.Attributes["Base"].Value);
        taxDto.Impuesto = taxItem.Attributes["Impuesto"].Value;
        taxDto.TipoFactor = taxItem.Attributes["TipoFactor"].Value;
        taxDto.TasaOCuota = Convert.ToDecimal(taxItem.Attributes["TasaOCuota"].Value);
        taxDto.Importe = Convert.ToDecimal(taxItem.Attributes["Importe"].Value);

        if (taxItem.Name == "cfdi:Traslado") {
          taxDto.MetodoAplicacion = BillTaxMethod.Traslado;

        } else if (taxItem.Name == "cfdi:Retencion") {
          taxDto.MetodoAplicacion = BillTaxMethod.Retencion;

        } else {

          throw Assertion.EnsureNoReachThisCode($"Unhandled SAT tax type: {taxNode.Name}");
        }
        taxesByConceptDto.Add(taxDto);
      }

      return taxesByConceptDto;
    }

    #endregion Helpers

  } // class SATCreditNoteXmlReader

} // namespace Empiria.Billing.SATMexicoImporter
