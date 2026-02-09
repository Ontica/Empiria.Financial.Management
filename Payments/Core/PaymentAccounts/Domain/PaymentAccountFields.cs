/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                          Component : Domain Layer                          *
*  Assembly : Empiria.Payments.Core.dll                    Pattern   : Input fields DTO                      *
*  Type     : PaymentAccountFields                         License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Input fields DTO used to update payment accounts.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial;

namespace Empiria.Payments {

  /// <summary>Input fields DTO used to update payment accounts.</summary>
  public class PaymentAccountFields {

    public string UID {
      get; set;
    } = string.Empty;


    public string AccountTypeUID {
      get; set;
    } = string.Empty;


    public string PaymentMethodUID {
      get; set;
    } = string.Empty;


    public string InstitutionUID {
      get; set;
    } = string.Empty;


    public string AccountNo {
      get; set;
    } = string.Empty;


    public string Identificator {
      get; set;
    } = string.Empty;


    public string CurrencyUID {
      get; set;
    } = string.Empty;


    public string HolderName {
      get; set;
    } = string.Empty;


    public bool AskForReferenceNumber {
      get; set;
    }


    public string ReferenceNumber {
      get; set;
    } = string.Empty;


    internal void EnsureValid() {
      UID = Patcher.CleanUID(UID);
      AccountTypeUID = Patcher.CleanUID(AccountTypeUID);
      PaymentMethodUID = Patcher.CleanUID(PaymentMethodUID);
      InstitutionUID = Patcher.CleanUID(InstitutionUID);
      CurrencyUID = Patcher.CleanUID(CurrencyUID);

      AccountNo = Patcher.PatchClean(AccountNo, string.Empty);
      Identificator = Patcher.PatchClean(Identificator, string.Empty);

      _ = PaymentAccountType.Parse(AccountTypeUID);
      _ = Currency.Parse(CurrencyUID);

      var paymentMethod = PaymentMethod.Parse(PaymentMethodUID);

      if (!paymentMethod.AccountRelated) {
        InstitutionUID = string.Empty;
        AccountNo = string.Empty;
        AskForReferenceNumber = false;
        ReferenceNumber = string.Empty;

        return;
      }

      Assertion.Require(InstitutionUID.Length != 0,
                       "Requiero se proporcione la institución financiera a la que pertenece la cuenta.");

      Assertion.Require(AccountNo.Length != 0,
                        "Requiero se proporcione el número de cuenta.");

      Assertion.Require(AccountNo.Length >= 5,
                        "El número de cuenta debe contener cuando menos 5 dígitos.");

      Assertion.Require(EmpiriaString.AllDigits(AccountNo),
                        "El número de cuenta debe conformarse únicamente por dígitos.");

    }

  }  // class PaymentAccountFields

}  // namespace Empiria.Payments
