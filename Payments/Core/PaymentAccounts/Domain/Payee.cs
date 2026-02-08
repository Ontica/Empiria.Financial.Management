/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                          Component : Domain Layer                          *
*  Assembly : Empiria.Payments.Core.dll                    Pattern   : Information Holder                    *
*  Type     : Payee                                        License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents a payee. A payee is someone who receives payments.                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Payments.Data;

namespace Empiria.Payments {

  /// <summary>Represents a payee. A payee is someone who receives payments.</summary>
  public class Payee : Party, INamedEntity {

    #region Constructors and parsers

    protected Payee(PartyType partyType) : base(partyType) {
      // Required by Empiria Framework.
    }

    internal Payee(PayeeFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      Update(fields);
    }

    static public new Payee Parse(int id) => ParseId<Payee>(id);

    static public new Payee Parse(string uid) => ParseKey<Payee>(uid);

    static public new Payee Empty => ParseEmpty<Payee>();

    #endregion Constructors and parsers

    #region Properties

    public PayeeType PayeeType {
      get {
        return PayeeType.Parse(ExtendedData.Get("supplierType", PayeeType.Unknown.Name));
      }
      private set {
        ExtendedData.SetIfValue("supplierType", value.Name);
      }
    }


    public string EmployeeNo {
      get {
        return ExtendedData.Get("employeeNo", string.Empty);
      }
      private set {
        ExtendedData.SetIfValue("employeeNo", value);
      }
    }


    public string SubledgerAccount {
      get {
        return ExtendedData.Get("taxData/subledgerAccount", string.Empty);
      }
      private set {
        ExtendedData.SetIfValue("taxData/subledgerAccount", value);
      }
    }


    public string SubledgerAccountName {
      get {
        return ExtendedData.Get("taxData/subledgerAccountName", string.Empty);
      }
      private set {
        ExtendedData.SetIfValue("taxData/subledgerAccountName", value);
      }
    }


    public string TaxCode {
      get {
        return Code;
      }
      private set {
        Code = value;
      }
    }

    string INamedEntity.Name {
      get {
        if (EmployeeNo.Length != 0) {
          return $"{Name} (No. expediente: {EmployeeNo})";
        }
        return $"{Name} ({TaxCode})";
      }
    }

    #endregion Properties

    #region Methods

    internal void Delete() {
      SetStatus(EntityStatus.Deleted);
    }


    protected override void OnBeforeSave() {
      if (IsNew) {
        PatchObjectId(PayeesData.GetNextPayeeId());
      }
    }


    internal void Update(PayeeFields fields) {
      Assertion.Require(fields, nameof(fields));

      PayeeType = PayeeType.Parse(fields.TypeUID);
      TaxCode = Patcher.Patch(fields.TaxCode, TaxCode);

      SubledgerAccount = Patcher.Patch(fields.SubledgerAccount, SubledgerAccount);
      SubledgerAccountName = Patcher.Patch(fields.SubledgerAccountName, SubledgerAccountName);

      EmployeeNo = EmpiriaString.Clean(fields.EmployeeNo);

      if (fields.UID.Length == 0) {
        fields.StartDate = DateTime.Today;
      } else {
        fields.StartDate = StartDate;
      }

      fields.EndDate = ExecutionServer.DateMaxValue;

      fields.Roles = EmpiriaString.Tagging("supplier")
                                  .ToArray();

      base.Update(fields);
    }

    #endregion Methods

  } // class Payee

} // namespace Empiria.Payments
