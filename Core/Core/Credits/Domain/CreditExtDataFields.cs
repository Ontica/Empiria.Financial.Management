/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : FinancialAccounts                            Component : Domain Layer                          *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Input fields                          *
*  Type     : CreditExtDataFields                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Input fields with financial credit information.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Financial {

  /// <summary>Input fields with financial credit information.</summary>
  public class CreditExtDataFields {

    #region Properties

    public string CreditoNo {
      get; set;
    } = string.Empty;


    //PFI_ETAPA_CRED
    public int EtapaCredito {
      get; set;
    } = 0;


    //PFI_ACREDITADO
    public string Acreditado {
      get; set;
    } = string.Empty;


    public string TipoCredito {
      get; set;
    } = string.Empty;


    public int Currency {
      get; set;
    } = 0;


    //PFI_INTERES_AL
    public decimal Interes {
      get; set;
    } = 0;


    //PFI_COMISIONES_AL
    public int Comision {
      get; set;
    } = 0;


    //PFI_SALDO
    public decimal Saldo {
      get; set;
    } = 0;


    //PFI_PLAZO_INVERSION
    public decimal PlazoInversion {
      get; set;
    } = 0;


    //PFI_PLAZO_GRACIA_INT
    public decimal PlazoGracia {
      get; set;
    } = 0;


    //PFI_PLAZO_AMORTIZACION
    public decimal PlazoAmortizacion {
      get; set;
    } = 0;


    //PFI_TASA
    public decimal Tasa {
      get; set;
    } = 0;


    //PFI_FACTOR_TASA
    public int FactorTasa {
      get; set;
    } = 0;


    public int TasaPiso {
      get; set;
    } = 0;


    public int TasaTecho {
      get; set;
    } = 0;


    //PFI_FECHA_AMORTIZA
    public DateTime FechaAmortizacion {
      get; set;
    } = DateTime.Now;

    #endregion Properties

    #region Methods

    #endregion Methods

  }  // class CreditExtDataFields

}  // namespace Empiria.Financial.Accounts
