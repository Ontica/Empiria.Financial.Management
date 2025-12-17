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

namespace Empiria.Billing.SATMexicoImporter {

  /// <summary>Service used to read from a bill as xml string and return
  /// general data to SATBillDto object</summary>
  internal class SATBillGeneralDataXmlReader {

    internal SATBillGeneralDataXmlReader() {

    }

    #region Public methods

    public SATBillGeneralDataDto GenerateGeneralData(XmlElement generalDataNode) {

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
        NoCertificado = GetAttribute(generalDataNode, "NoCertificado"),
        Certificado = GetAttribute(generalDataNode, "Certificado"),
        SubTotal = GetAttribute<decimal>(generalDataNode, "SubTotal"),
        Moneda = GetAttribute(generalDataNode, "Moneda"),
        Total = GetAttribute<decimal>(generalDataNode, "Total"),

        TipoCambio = GetAttribute(generalDataNode, "TipoCambio"),
        TipoDeComprobante = GetAttribute(generalDataNode, "TipoDeComprobante"),
        Exportacion = GetAttribute(generalDataNode, "Exportacion"),
        MetodoPago = GetAttribute(generalDataNode, "MetodoPago"),
        LugarExpedicion = GetAttribute(generalDataNode, "LugarExpedicion"),
      };

    }

    #endregion Public methods


    #region Services

    internal SATBillOrganizationDto GenerateReceiverData(XmlNode receiverNode) {

      return new SATBillOrganizationDto {
        RegimenFiscal = GetAttribute(receiverNode, "RegimenFiscalReceptor"),
        RFC = GetAttribute(receiverNode, "Rfc"),
        Nombre = GetAttribute(receiverNode, "Nombre"),
        DomicilioFiscal = GetAttribute(receiverNode, "DomicilioFiscalReceptor"),
        UsoCFDI = GetAttribute(receiverNode, "UsoCFDI"),
      };
    }


    internal SATBillOrganizationDto GenerateSenderData(XmlNode senderNode) {

      return new SATBillOrganizationDto {
        RegimenFiscal = GetAttribute(senderNode, "RegimenFiscal"),
        RFC = GetAttribute(senderNode, "Rfc"),
        Nombre = GetAttribute(senderNode, "Nombre"),
      };
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


    internal IEnumerable<SATBillTaxDto> GetTaxItems(XmlNode taxNode) {
      var taxesByConceptDto = new List<SATBillTaxDto>();

      foreach (XmlNode taxItem in taxNode.ChildNodes) {
        var taxDto = new SATBillTaxDto();

        taxDto.Base = GetAttribute<decimal>(taxItem, "Base");
        taxDto.Impuesto = GetAttribute(taxItem, "Impuesto");
        taxDto.TipoFactor = GetAttribute(taxItem, "TipoFactor");
        taxDto.TasaOCuota = GetAttribute<decimal>(taxItem, "TasaOCuota");
        taxDto.Importe = GetAttribute<decimal>(taxItem, "Importe");

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

    #endregion Services

  } // class SATBillGeneralDataXmlReader

} // namespace Empiria.Billing.SATMexicoImporter
