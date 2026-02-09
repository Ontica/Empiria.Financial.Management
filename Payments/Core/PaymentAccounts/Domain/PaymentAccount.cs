/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                          Component : Domain Layer                          *
*  Assembly : Empiria.Payments.Core.dll                    Pattern   : Information Holder                    *
*  Type     : PaymentAccount                               License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Holds information about a payee's payment account.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Financial;
using Empiria.Json;
using Empiria.Parties;
using Empiria.Payments.Data;
using Empiria.StateEnums;

namespace Empiria.Payments {

  /// <summary>Holds information about a payee's payment account.</summary>
  public class PaymentAccount : BaseObject, INamedEntity {

    #region Constructors and parsers

    protected PaymentAccount() {
      // Required by Empiria Framework.
    }

    public PaymentAccount(Payee payee, PaymentAccountFields fields) {
      Assertion.Require(payee, nameof(payee));
      Assertion.Require(!payee.IsEmptyInstance, nameof(payee));
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      Payee = payee;

      Update(fields);
    }

    static public PaymentAccount Parse(int id) => ParseId<PaymentAccount>(id);

    static public PaymentAccount Parse(string uid) => ParseKey<PaymentAccount>(uid);

    static public FixedList<PaymentAccount> GetListFor(Payee payee) {
      return GetList<PaymentAccount>()
            .ToFixedList()
            .FindAll(x => x.Payee.Equals(payee));
    }

    static public PaymentAccount Empty => ParseEmpty<PaymentAccount>();

    #endregion Constructors and parsers

    #region Properties

    [DataField("PYMT_ACCT_TYPE_ID")]
    public PaymentAccountType AccountType {
      get; private set;
    }


    string INamedEntity.Name {
      get {
        string temp = Institution.IsEmptyInstance ? PaymentMethod.Name : Institution.CommonName;

        if (Identificator.Length != 0) {
          temp = $"{temp} ({Identificator})";
        }
        if (AccountNo.Length == 0) {
          return temp;
        }
        return $"{temp} - {EmpiriaString.TruncateLast(AccountNo, 6)}";
      }
    }

    [DataField("PYMT_ACCT_PARTY_ID")]
    public Payee Payee {
      get; private set;
    }


    [DataField("PYMT_ACCT_METHOD_ID")]
    public PaymentMethod PaymentMethod {
      get; private set;
    }


    [DataField("PYMT_ACCT_INSTITUTION_ID")]
    public FinancialInstitution Institution {
      get; private set;
    }


    [DataField("PYMT_ACCT_CURRENCY_ID")]
    public Currency Currency {
      get; private set;
    }


    [DataField("PYMT_ACCT_NUMBER")]
    public string AccountNo {
      get; private set;
    }

    public string Identificator {
      get {
        return ExtData.Get("identificator", string.Empty);
      }
      private set {
        ExtData.SetIf("identificator", value, value.Length != 0 &&
                                              value != ((INamedEntity) this).Name);
      }
    }


    public string HolderName {
      get {
        return ExtData.Get("holderName", Payee.Name);
      }
      private set {
        ExtData.SetIf("holderName", value, value.Length != 0 && value != Payee.Name);
      }
    }

    public string ReferenceNumber {
      get {
        return ExtData.Get("referenceNumber", string.Empty);
      }
      private set {
        ExtData.SetIfValue("referenceNumber", value);
      }
    }


    [DataField("PYMT_ACCT_EXT_DATA")]
    internal JsonObject ExtData {
      get; private set;
    }


    [DataField("PYMT_ACCT_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("PYMT_ACCT_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("PYMT_ACCT_STATUS", Default = EntityStatus.Active)]
    public EntityStatus Status {
      get; private set;
    }


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(AccountNo, HolderName, Payee.Keywords,
                                           Institution.Keywords, Identificator);
      }
    }

    public bool AskForReferenceNumber {
      get {
        return ExtData.Get("askForReferenceNumber", false);
      }
      private set {
        ExtData.SetIf("askForReferenceNumber", value, value == true);
      }
    }

    #endregion Properties

    #region Methods

    public void Delete() {
      Status = EntityStatus.Deleted;

      MarkAsDirty();
    }


    protected override void OnSave() {
      if (IsNew) {
        PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        PostingTime = DateTime.Now;
      }

      PaymentAccountsData.Write(this);
    }


    public void Update(PaymentAccountFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      AccountType = Patcher.Patch(fields.AccountTypeUID, AccountType);
      PaymentMethod = Patcher.Patch(fields.PaymentMethodUID, PaymentMethod);
      Institution = Patcher.Patch(fields.InstitutionUID, Institution);
      AccountNo = EmpiriaString.Clean(fields.AccountNo);
      Identificator = EmpiriaString.Clean(fields.Identificator);
      Currency = Patcher.Patch(fields.CurrencyUID, Currency);
      HolderName = EmpiriaString.Clean(fields.HolderName);
      AskForReferenceNumber = fields.AskForReferenceNumber;
      ReferenceNumber = EmpiriaString.Clean(fields.ReferenceNumber);

      MarkAsDirty();
    }

    #endregion Methods

  }  // class PaymentAccount

}  // namespace Empiria.Payments
