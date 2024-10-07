/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Domain Layer                            *
*  Assembly : Empiria.Contracts.Core.dll                 Pattern   : Infomation Holder                       *
*  Type     : ContractDocumentType                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a contract document type.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Contracts {

  /// <summary>Represents a contract document type.</summary>
  public class ContractDocumentType : GeneralObject {

    #region Constructors and parsers

    static internal ContractDocumentType Parse(int id) {
      return BaseObject.ParseId<ContractDocumentType>(id);
    }

    static internal ContractDocumentType Parse(string uid) {
      return BaseObject.ParseKey<ContractDocumentType>(uid);
    }

    static internal FixedList<ContractDocumentType> GetList() {
      return BaseObject.GetList<ContractDocumentType>()
                       .ToFixedList();
    }

    static internal ContractDocumentType Empty => BaseObject.ParseEmpty<ContractDocumentType>();

    #endregion Constructors and parsers

  }  // class ContractDocumentType

}  // namespace Empiria.Contracts
