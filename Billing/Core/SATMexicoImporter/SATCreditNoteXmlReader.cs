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

          _satCreditNoteDto.DatosGenerales.CfdiRelacionados = generalDataReader.GenerateRelatedCfdiData(node);
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

    
    private void GenerateComplementData(XmlNode complementNode) {

      foreach (XmlNode childNode in complementNode.ChildNodes) {

        if (childNode.Name.Equals("tfd:TimbreFiscalDigital")) {

          _satCreditNoteDto.SATComplemento = generalDataReader.GenerateSATComplementData(childNode);
        }

        if (childNode.Name.Equals("leyendasFisc:LeyendasFiscales")) {

          _satCreditNoteDto.LeyendasFiscales = generalDataReader.GenerateLeyendasFiscales(childNode);
        }
        if (childNode.Name.Equals("implocal:ImpuestosLocales")) {

          _satCreditNoteDto.ImpuestosLocales = generalDataReader.GenerateImpuestosLocales(childNode);
        }
      }
    }

    #endregion Helpers

  } // class SATCreditNoteXmlReader

} // namespace Empiria.Billing.SATMexicoImporter
