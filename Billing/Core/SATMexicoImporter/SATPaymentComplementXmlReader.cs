using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Empiria.Billing.SATMexicoImporter {
  internal class SATPaymentComplementXmlReader {

    private readonly SATBillDto _satCreditNoteDto;

    private readonly XmlDocument _xmlDocument;

    internal SATPaymentComplementXmlReader(string xmlString) {
      Assertion.Require(xmlString, nameof(xmlString));

      _satCreditNoteDto = new SATBillDto();

      _xmlDocument = new XmlDocument();

      _xmlDocument.LoadXml(xmlString);
    }


    #region Services

    internal SATBillDto ReadAsPaymentComplementDto() {

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

      var conceptosDto = new List<SATBillConceptDto>();

      foreach (XmlNode concept in conceptsNode.ChildNodes) {

        if (!concept.Name.Equals("cfdi:Concepto")) {
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
          ObjetoImp = GetAttribute(concept, "ObjetoImp"),
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


    private string GetAttribute(XmlNode concept, string attributeName) {
      return FieldPatcher.PatchField(concept.Attributes[attributeName]?.Value, string.Empty);
    }


    private T GetAttribute<T>(XmlNode concept, string attributeName) {
      if (concept.Attributes[attributeName]?.Value == null) {
        return default;
      } else {
        return (T) Convert.ChangeType(concept.Attributes[attributeName]?.Value, typeof(T));
      }
    }

    #endregion Helpers
  }
}
