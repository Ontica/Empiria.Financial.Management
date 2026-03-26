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
        if (node.Name == "cfdi:Impuestos") {

          _satBillDto.GeneralTaxes = generalDataReader.GenerateGeneralTaxesData(node);
        }
        if (node.Name == "cfdi:Complemento") {

          GenerateComplementData(node);
        }
        if (node.Name == "cfdi:Addenda") {

          GenerateAddendaComplementData(node);
        }
      }

      return _satBillDto;
    }

    #endregion Services

    #region Helpers

    private void GenerateAddendaComplementData(XmlNode complementNode) {

      foreach (XmlNode complementChild in complementNode.ChildNodes) {

        if (complementChild.Name.Equals("eco:Complementaria")) {

          _satBillDto.Addenda = new SATBillAddenda {
            NoEstacion = generalDataReader.GetAttribute(complementChild, "noEstacion"),
            ClavePemex = generalDataReader.GetAttribute(complementChild, "clavePemex"),
            AddendaConcepts = GenerateAddendaEcoConcepts(complementChild.ChildNodes)
          };

        } else if (complementChild.Name.Equals("TOKA")) {

          _satBillDto.Addenda = new SATBillAddenda {
            Serie = generalDataReader.GetAttribute(complementChild, "Serie"),
            Folio = generalDataReader.GetAttribute(complementChild, "Folio"),
            FechaEmision = generalDataReader.GetAttribute<DateTime>(complementChild, "FechaEmision"),
          };

          if (complementChild.FirstChild.Name.Equals("Concepto")) {

            _satBillDto.Addenda.Concepto = new SATBillConceptDto {
              Cantidad = generalDataReader.GetAttribute<decimal>(complementChild.FirstChild, "Cantidad"),
              Descripcion = generalDataReader.GetAttribute(complementChild.FirstChild, "Descripcion"),
              Importe = generalDataReader.GetAttribute<decimal>(complementChild.FirstChild, "Importe")

            };
          }
        } else if (complementChild.Name.EndsWith(":minotaria")) {

          foreach (XmlNode item in complementChild.ChildNodes) {

            if (item.Name.EndsWith(":Conceptos")) {
              _satBillDto.Addenda = new SATBillAddenda {
                Conceptos = GenerateBillConceptsFromAddenda(item, true)
              };
            }
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


    private FixedList<SATBillConceptDto> GenerateBillConceptsFromAddenda(XmlNode conceptsNode,
                                                                          bool isConceptSumToTotal = false) {
      var conceptosDto = new List<SATBillConceptDto>();

      foreach (XmlNode concept in conceptsNode.ChildNodes) {

        if (!concept.Name.EndsWith(":Concepto")) {
          Assertion.EnsureFailed("The concepts node must contain only concepts.");
        }

        var conceptoDto = new SATBillConceptDto() {
          IsConceptSumToTotal = isConceptSumToTotal,
          Cantidad = generalDataReader.GetAttribute<decimal>(concept, "cantidad"),
          Unidad = generalDataReader.GetAttribute(concept, "unidad"),
          ValorUnitario = generalDataReader.GetAttribute<decimal>(concept, "valorUnitario"),
          Importe = generalDataReader.GetAttribute<decimal>(concept, "importe"),
          Descripcion = generalDataReader.GetAttribute(concept, "descripcion"),
          ClaveProdServ = generalDataReader.GetAttribute(concept, "claveProdServ"),
          ClaveUnidad = generalDataReader.GetAttribute(concept, "claveUnidad"),
          NoIdentificacion = generalDataReader.GetAttribute(concept, "noIdentificacion"),
          Descuento = generalDataReader.GetAttribute<decimal>(concept, "descuento"),
          ObjetoImp = generalDataReader.GetAttribute(concept, "objetoImp"),
          Impuestos = generalDataReader.GenerateTaxesByConcept(concept.ChildNodes)
        };

        conceptosDto.Add(conceptoDto);
      }
      return conceptosDto.ToFixedList();
    }


    private void GenerateComplementData(XmlNode complementNode) {

      foreach (XmlNode childNode in complementNode.ChildNodes) {

        if (childNode.Name.Equals("tfd:TimbreFiscalDigital")) {

          _satBillDto.SATComplemento = generalDataReader.GenerateSATComplementData(childNode);
        }
        if (childNode.Name.Equals("leyendasFisc:LeyendasFiscales")) {

          _satBillDto.LeyendasFiscales = generalDataReader.GenerateLeyendasFiscales(childNode);
        }
        if (childNode.Name.Equals("implocal:ImpuestosLocales")) {

          _satBillDto.ImpuestosLocales = generalDataReader.GenerateImpuestosLocales(childNode);
        }
      }
    }

    #endregion Helpers

  } // class SATBillXmlReader

} // namespace Empiria.Billing.SATMexicoImporter
