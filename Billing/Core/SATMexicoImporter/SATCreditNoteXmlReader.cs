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

          _satCreditNoteDto.Conceptos = generalDataReader.GenerateConceptsList(node);
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

      foreach (XmlNode childNode in complementNode.ChildNodes) {

        _satCreditNoteDto.SATComplemento = generalDataReader.GenerateSATComplementData(childNode);

        if (childNode.Name.Equals("leyendasFisc:LeyendasFiscales")) {

          _satCreditNoteDto.LeyendasFiscales = generalDataReader.GenerateLeyendasFiscales(childNode);
        }
      }
    }

    #endregion Helpers

  } // class SATCreditNoteXmlReader

} // namespace Empiria.Billing.SATMexicoImporter
