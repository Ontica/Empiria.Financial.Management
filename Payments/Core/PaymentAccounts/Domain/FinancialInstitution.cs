/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                          Component : Domain Layer                          *
*  Assembly : Empiria.Payments.Core.dll                    Pattern   : Common Storage Type                   *
*  Type     : FinancialInstitution                         License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents a financial institution like a bank or a payments broker.                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments {

  /// <summary>Represents a financial institution like a bank or a payments broker.</summary>
  public class FinancialInstitution : CommonStorage, INamedEntity {

    #region Constructors and parsers

    protected FinancialInstitution() {
      // Required by Empiria Framework.
    }

    static public FinancialInstitution Parse(int id) => ParseId<FinancialInstitution>(id);

    static public FinancialInstitution Parse(string uid) => ParseKey<FinancialInstitution>(uid);

    static public FinancialInstitution Empty => ParseEmpty<FinancialInstitution>();

    static public FixedList<FinancialInstitution> GetList() {
      return GetStorageObjects<FinancialInstitution>();
    }

    #endregion Constructors and parsers

    #region Properties

    public string BrokerCode {
      get {
        return Code;
      }
    }

    public string CommonName {
      get {
        return ExtData.Get("commonName", base.Name);
      }
    }


    string INamedEntity.Name {
      get {
        if (Code.Length != 0) {
          return $"{base.Name} ({Code})";
        }
        return base.Name;
      }
    }

    #endregion Properties

  } // class FinancialInstitution

} // namespace Empiria.Payments
