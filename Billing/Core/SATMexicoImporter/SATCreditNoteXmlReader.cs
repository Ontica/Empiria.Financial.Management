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

    private readonly SATBillGeneralDataXmlReader generalDataReader;

    internal SATCreditNoteXmlReader(string xmlString) {
      Assertion.Require(xmlString, nameof(xmlString));

      _satCreditNoteDto = new SATBillDto();

      _xmlDocument = new XmlDocument();

      _xmlDocument.LoadXml(xmlString);

      generalDataReader = new SATBillGeneralDataXmlReader();
    }

    #region Services

    internal SATBillDto ReadAsCreditNoteDto() {

      XmlElement generalData = _xmlDocument.DocumentElement;
      XmlNodeList nodes = generalData.ChildNodes;

      _satCreditNoteDto.DatosGenerales = generalDataReader.GenerateGeneralData(generalData);

      foreach (XmlNode node in nodes) {

        if (node.Name == "cfdi:CfdiRelacionados") {

          GenerateCfdiRelatedData(node);
        }
        if (node.Name == "cfdi:Emisor") {

          _satCreditNoteDto.Emisor = generalDataReader.GenerateSenderData(node);
        }
        if (node.Name == "cfdi:Receptor") {

          _satCreditNoteDto.Receptor = generalDataReader.GenerateReceiverData(node);
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
          UUID = generalDataReader.GetAttribute(cfdiRelated, "UUID"),
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
        Xmlns_Tfd = generalDataReader.GetAttribute(timbre, "xmlns:tfd"),
        Xmlns_Xsi = generalDataReader.GetAttribute(timbre, "xmlns:xsi"),
        Xsi_SchemaLocation = generalDataReader.GetAttribute(timbre, "xsi:schemaLocation"),
        Tfd_Version = generalDataReader.GetAttribute(timbre, "Version"),
        UUID = generalDataReader.GetAttribute(timbre, "UUID"),
        FechaTimbrado = generalDataReader.GetAttribute<DateTime>(timbre, "FechaTimbrado"),
        RfcProvCertif = generalDataReader.GetAttribute(timbre, "RfcProvCertif"),
        SelloCFD = generalDataReader.GetAttribute(timbre, "SelloCFD"),
        NoCertificadoSAT = generalDataReader.GetAttribute(timbre, "NoCertificadoSAT"),
        SelloSAT = generalDataReader.GetAttribute(timbre, "SelloSAT")
      };
    }


    private void GenerateConceptsList(XmlNode conceptsNode) {

      var conceptosDto = new List<SATBillConceptWithTaxDto>();

      foreach (XmlNode concept in conceptsNode.ChildNodes) {

        if (!concept.Name.Equals("cfdi:Concepto")) {
          Assertion.EnsureFailed("The concepts node must contain only concepts.");
        }

        var conceptoDto = new SATBillConceptWithTaxDto() {
          ClaveProdServ = generalDataReader.GetAttribute(concept, "ClaveProdServ"),
          ClaveUnidad = generalDataReader.GetAttribute(concept, "ClaveUnidad"),
          Cantidad = generalDataReader.GetAttribute<decimal>(concept, "Cantidad"),
          Unidad = generalDataReader.GetAttribute(concept, "Unidad"),
          NoIdentificacion = generalDataReader.GetAttribute(concept, "NoIdentificacion"),
          Descripcion = generalDataReader.GetAttribute(concept, "Descripcion"),
          ValorUnitario = generalDataReader.GetAttribute<decimal>(concept, "ValorUnitario"),
          Importe = generalDataReader.GetAttribute<decimal>(concept, "Importe"),
          ObjetoImp = generalDataReader.GetAttribute(concept, "ObjetoImp"),
          Impuestos = GenerateTaxesByConcept(concept.ChildNodes.Item(0))
        };

        conceptosDto.Add(conceptoDto);

      }
      _satCreditNoteDto.Conceptos = conceptosDto.ToFixedList();
    }


    private FixedList<SATBillTaxDto> GenerateTaxesByConcept(XmlNode taxesNode) {

      var taxesByConceptDto = new List<SATBillTaxDto>();

      foreach (XmlNode taxNode in taxesNode.ChildNodes) {

        taxesByConceptDto.AddRange(generalDataReader.GetTaxItems(taxNode));
      }
      return taxesByConceptDto.ToFixedList();
    }

    #endregion Helpers

  } // class SATCreditNoteXmlReader

} // namespace Empiria.Billing.SATMexicoImporter
