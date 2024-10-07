/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Contracts.Core.dll                 Pattern   : Data Transfer Object                    *
*  Type     : ContractDocumentDto                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer object used to return contracts document information.                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Contracts.Adapters {

  /// <summary>Data transfer object used to return contracts document information.</summary>
  public class ContractDocumentDto {

    public string UID {
      get; internal set;
    }


    public string Name {
      get; internal set;
    }


    public NamedEntityDto DocSourceType {
      get; internal set;
    }


    public string Path {
      get; internal set;
    }


    public string Directory {
      get; internal set;
    }


    public NamedEntityDto SourceOject {
      get; internal set;
    }

    public NamedEntityDto DocEmmitedBy {
      get; internal set;
    }


    public string ExtData {
      get; internal set;
    }


    public string KeyWords {
      get; internal set;
    }


    public string PostedBy {
      get; internal set;
    }


    public string PostingTime {
      get; internal set;
    }


    public string Status {
      get; internal set;
    }


  }  // class ContractDocumentDto

}  // namespace Empiria.Contracts.Adapters
