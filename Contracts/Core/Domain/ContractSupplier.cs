/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Domain Layer                            *
*  Assembly : Empiria.Contracts.Core.dll                 Pattern   : Infomation Holder                       *
*  Type     : ContractSupplier                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a contract supplier.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;

namespace Empiria.Contracts {

  /// <summary>Represents a contract suppliers.</summary>
  public class ContractSupplier : Party {

    #region Constructors and parsers

    static internal new ContractSupplier Parse(int id) {
      return ParseId<ContractSupplier>(id);
    }

    static internal new ContractSupplier Parse(string uid) {
      return ParseKey<ContractSupplier>(uid);
    }

    static internal FixedList<ContractSupplier> GetList() {
      return GetList<ContractSupplier>().ToFixedList();
    }

    static internal new ContractSupplier Empty => ParseEmpty<ContractSupplier>();

    #endregion Constructors and parsers

  }  // class ContractSupplier

}  // namespace Empiria.Contracts
