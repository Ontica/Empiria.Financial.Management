/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Domain Layer                            *
*  Assembly : Empiria.Contracts.Core.dll                 Pattern   : Infomation Holder                       *
*  Type     : ContractType                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Describes a contract type.                                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Contracts {

  /// <summary>Describes a contract type.</summary>
  public class ContractType : GeneralObject {

    #region Constructors and parsers

    static internal ContractType Parse(int id) {
      return ParseId<ContractType>(id);
    }

    static internal ContractType Parse(string uid) {
      return ParseKey<ContractType>(uid);
    }

    static internal FixedList<ContractType> GetList() {
      return GetList<ContractType>().ToFixedList();
    }

    static internal ContractType Empty => ParseEmpty<ContractType>();

    #endregion Constructors and parsers

  }  // class ContractType

}  // namespace Empiria.Contracts
