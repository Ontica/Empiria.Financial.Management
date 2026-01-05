/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing SATMexico Importer                 Component : Interface adapters                      *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Output DTO                              *
*  Type     : SATFuelConsumptionBillDto                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return a SAT Mexico bill object.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Billing.SATMexicoImporter {

  /// <summary>Output DTO used to return a SAT Mexico bill object</summary>
  internal class SATFuelConsumptionBillDto : ISATBillDto {

    public SATBillGeneralDataDto DatosGenerales {
      get; internal set;
    } = new SATBillGeneralDataDto();


    public SATBillOrganizationDto Emisor {
      get; internal set;
    } = new SATBillOrganizationDto();


    public SATBillOrganizationDto Receptor {
      get; internal set;
    } = new SATBillOrganizationDto();


    public FixedList<SATBillConceptDto> Conceptos {
      get; internal set;
    } = new FixedList<SATBillConceptDto>();


    public FuelConsumptionComplementDataDto DatosComplemento {
      get; internal set;
    } = new FuelConsumptionComplementDataDto();


    public SATBillComplementDto SATComplemento {
      get; internal set;
    } = new SATBillComplementDto();


    public SATFuelConsumptionAddendaDto Addenda {
      get; internal set;
    } = new SATFuelConsumptionAddendaDto();

  } // class SATFuelConsumptionBillDto


  internal class SATFuelConsumptionAddendaDto {

    public FixedList<string> AddendaLeyendas {
      get; internal set;
    } = new FixedList<string>();


    public FixedList<SATBillConceptDto> AddendaConceptos {
      get; internal set;
    } = new FixedList<SATBillConceptDto>();

  } // SATFuelConsumptionAddendaDto


  internal class FuelConsumptionComplementDataDto {

    public string Version {
      get; internal set;
    }


    public string TipoOperacion {
      get; internal set;
    }


    public string NumeroDeCuenta {
      get; internal set;
    }


    public decimal SubTotal {
      get; internal set;
    }


    public decimal Total {
      get; internal set;
    }


    public FixedList<FuelConsumptionComplementConceptDataDto> ComplementoConceptos {
      get; internal set;
    } = new FixedList<FuelConsumptionComplementConceptDataDto>();

  } // class FuelConsumptionComplementDataDto


  internal class FuelConsumptionComplementConceptDataDto {

    public string Identificador {
      get; internal set;
    }


    public string Rfc {
      get; internal set;
    }


    public string ClaveEstacion {
      get; internal set;
    }


    public string TipoCombustible {
      get; internal set;
    }


    public string Unidad {
      get; internal set;
    }


    public string NombreCombustible {
      get; internal set;
    }


    public string FolioOperacion {
      get; internal set;
    }


    public DateTime Fecha {
      get; internal set;
    }


    public decimal Cantidad {
      get; internal set;
    }


    public decimal ValorUnitario {
      get; internal set;
    }


    public decimal Importe {
      get; internal set;
    }


    public FixedList<SATBillTaxDto> Impuestos {
      get; set;
    } = new FixedList<SATBillTaxDto>();

  } // class FuelConsumptionComplementConceptDataDto

} // namespace Empiria.Billing.SATMexicoImporter
