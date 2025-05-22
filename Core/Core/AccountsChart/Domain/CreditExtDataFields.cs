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

namespace Empiria.Financial.Accounts {

  /// <summary>Input fields with financial credit information.</summary>
  public class CreditExtDataFields {

    #region Properties
        
    public string CreditNo {
      get; set;
    } = string.Empty;


    //PFI_ETAPA_CRED
    public int EtapaCredito {
      get; set;
    } = 0;


    //PFI_ACREDITADO
    public string Acredited {
      get; set;
    } = string.Empty;

        
    public int CreditType {
      get; set;
    } = 0;

        
    public int Currency {
      get; set;
    } = 0;


    //PFI_INTERES_AL
    public decimal Interests {
      get; set;
    } = 0;


    //PFI_COMISIONES_AL
    public int Commissions {
      get; set;
    } = 0;


    //PFI_SALDO
    public decimal Balances {
      get; set;
    } = 0;


    //PFI_PLAZO_INVERSION
    public decimal InvestmentTerm {
      get; set;
    } = 0;


    //PFI_PLAZO_GRACIA_INT
    public decimal PlazoGracia {
      get; set;
    } = 0;


    //PFI_PLAZO_AMORTIZACION
    public decimal AmortizationTerm {
      get; set;
    } = 0;


    //PFI_TASA
    public decimal Rate {
      get; set;
    } = 0;


    //PFI_FACTOR_TASA
    public int RateFactor {
      get; set;
    } = 0;


    public int TasaPiso {
      get; set;
    } = 0;


    public int TasaTecho {
      get; set;
    } = 0;


    //PFI_FECHA_AMORTIZA
    public DateTime AmortizationDate {
      get; set;
    } = DateTime.Now;
    
    #endregion Properties

    #region Methods

    #endregion Methods

  }  // class CreditExtDataFields

}  // namespace Empiria.Financial.Accounts
