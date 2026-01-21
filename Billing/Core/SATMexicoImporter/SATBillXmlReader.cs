/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing SATMexico Importer                 Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Service provider                        *
*  Type     : SATBillXmlReader                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Service used to read a bill as xml string and return a SATBillDto object.                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using System.Xml;

namespace Empiria.Billing.SATMexicoImporter {

  /// <summary>Service used to read a bill as xml string and return a SATBillDto object.</summary>
  public class SATBillXmlReader {

    private readonly SATBillDto _satBillDto;

    private readonly XmlDocument _xmlDocument;

    private readonly SATBillGeneralDataXmlReader generalDataReader;

    internal SATBillXmlReader(string xmlString) {
      Assertion.Require(xmlString, nameof(xmlString));

      _satBillDto = new SATBillDto();

      _xmlDocument = new XmlDocument();

      _xmlDocument.LoadXml(xmlString);

      generalDataReader = new SATBillGeneralDataXmlReader();
    }

    #region Services

    internal SATBillDto ReadAsBillDto() {

      XmlElement generalData = _xmlDocument.DocumentElement;
      XmlNodeList nodes = generalData.ChildNodes;

      _satBillDto.DatosGenerales = generalDataReader.GenerateGeneralData(generalData);

      foreach (XmlNode node in nodes) {

        if (node.Name == "cfdi:CfdiRelacionados") {

          _satBillDto.DatosGenerales.CfdiRelacionados = generalDataReader.GenerateRelatedCfdiData(node);
        }
        if (node.Name == "cfdi:Emisor") {

          _satBillDto.Emisor = generalDataReader.GenerateSenderData(node);
        }
        if (node.Name == "cfdi:Receptor") {

          _satBillDto.Receptor = generalDataReader.GenerateReceiverData(node);
        }
        if (node.Name == "cfdi:Conceptos") {

          _satBillDto.Conceptos = generalDataReader.GenerateConceptsList(node);
        }
        if (node.Name == "cfdi:Complemento") {
          
          GenerateComplementData(node);
        }
        if (node.Name == "cfdi:Addenda") {

          GenerateAddendaEcoComplementData(node);
        }
      }

      return _satBillDto;
    }

    #endregion Services

    #region Helpers

    private void GenerateAddendaEcoComplementData(XmlNode complementNode) {

      foreach (XmlNode child in complementNode.ChildNodes) {

        if (child.Name.Equals("eco:Complementaria")) {

          _satBillDto.Addenda = new SATBillAddenda {
            NoEstacion = generalDataReader.GetAttribute(child, "noEstacion"),
            ClavePemex = generalDataReader.GetAttribute(child, "clavePemex"),
            EcoConcepts = GenerateAddendaEcoConcepts(child.ChildNodes)
          };

        } else if (child.Name.Equals("TOKA")) {

          _satBillDto.Addenda = new SATBillAddenda {
            Serie = generalDataReader.GetAttribute(child, "Serie"),
            Folio = generalDataReader.GetAttribute(child, "Folio"),
            FechaEmision = generalDataReader.GetAttribute<DateTime>(child, "FechaEmision"),
          };

          if (child.FirstChild.Name.Equals("Concepto")) {

            _satBillDto.Addenda.Concepto = new SATBillConceptDto {
              Cantidad = generalDataReader.GetAttribute<decimal>(child.FirstChild, "Cantidad"),
              Descripcion = generalDataReader.GetAttribute(child.FirstChild, "Descripcion"),
              Importe = generalDataReader.GetAttribute<decimal>(child.FirstChild, "Importe")

            };
          }
        }
      }
    }


    private FixedList<SATBillAddendaConcept> GenerateAddendaEcoConcepts(XmlNodeList ecoComplements) {

      var addendaConcepts = new List<SATBillAddendaConcept>();

      foreach (XmlNode ecoComplement in ecoComplements) {

        if (ecoComplement.Name.Equals("eco:Conceptos")) {

          XmlNode ecoConcept = ecoComplement.FirstChild;

          if (ecoConcept.Name.Equals("eco:Concepto")) {

            addendaConcepts.Add(
              new SATBillAddendaConcept {
                TasaIEPS = generalDataReader.GetAttribute<decimal>(ecoConcept, "tasaIeps"),
                IEPS = generalDataReader.GetAttribute<decimal>(ecoConcept, "ieps"),
                TasaIVA = generalDataReader.GetAttribute<decimal>(ecoConcept, "tasaIva"),
                IVA = generalDataReader.GetAttribute<decimal>(ecoConcept, "iva"),
                NoIdentificacion = generalDataReader.GetAttribute(ecoConcept, "noIdentificacion"),
                TasaAIEPS = generalDataReader.GetAttribute<decimal>(ecoConcept, "tasaAieps"),
                AIEPS = generalDataReader.GetAttribute<decimal>(ecoConcept, "aIeps")
              }
            );
          }
        }
      }
      return addendaConcepts.ToFixedList();
    }


    private void GenerateComplementData(XmlNode complementNode) {

      foreach (XmlNode childNode in complementNode.ChildNodes) {

        if (childNode.Name.Equals("tfd:TimbreFiscalDigital")) {

          _satBillDto.SATComplemento = generalDataReader.GenerateSATComplementData(childNode);
        }
        if (childNode.Name.Equals("leyendasFisc:LeyendasFiscales")) {

          _satBillDto.LeyendasFiscales = generalDataReader.GenerateLeyendasFiscales(childNode);
        }
      }
    }

    #endregion Helpers

  } // class SATBillXmlReader

} // namespace Empiria.Billing.SATMexicoImporter
