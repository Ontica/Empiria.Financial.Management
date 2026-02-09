/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Output DTO                              *
*  Type     : PaymentAccountDto                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO for PaymentAccount instances.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments.Adapters {

  /// <summary>Output DTO for PaymentAccount instances.</summary>
  public class PaymentAccountDto : NamedEntityDto {

    public PaymentAccountDto(PaymentAccount account) : base(account) {
      AccountType = account.AccountType.MapToNamedEntity();
      PaymentMethod = new PaymentMethodDto(account.PaymentMethod);
      Institution = account.Institution.MapToNamedEntity();
      AccountNo = account.AccountNo;
      Currency = account.Currency.MapToNamedEntity();
      Identificator = account.Identificator;
      HolderName = account.HolderName;
      ReferenceNumber = account.ReferenceNumber;
      AskForReferenceNumber = account.AskForReferenceNumber;
    }

    #region Properties

    public NamedEntityDto AccountType {
      get;
    }

    public PaymentMethodDto PaymentMethod {
      get;
    }

    public NamedEntityDto Institution {
      get;
    }

    public string AccountNo {
      get;
    }

    public NamedEntityDto Currency {
      get;
    }

    public string Identificator {
      get;
    }

    public string HolderName {
      get;
    }

    public string ReferenceNumber {
      get;
    }

    public bool AskForReferenceNumber {
      get;
    }

    #endregion Properties

    #region Mappers

    static public FixedList<PaymentAccountDto> Map(FixedList<PaymentAccount> accounts) {
      return accounts.Select(x => new PaymentAccountDto(x))
                     .ToFixedList();
    }

    #endregion Mappers

  }  // class PaymentAccountDto

}  // namespace Empiria.Payments.Adapters
