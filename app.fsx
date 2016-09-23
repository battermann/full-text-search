#r @"packages/Suave/lib/net40/Suave.dll"
#r @"packages/FSharp.Data.TypeProviders/lib/net40/FSharp.Data.TypeProviders.dll"
#r @"System.Data.Linq.dll"

open Suave
open Suave.Successful
open Suave.Operators
open Suave.Filters
open Writers
open System.Text
open System
open Suave.Files
open Suave.RequestErrors
open System.Linq
open System.Data
open System.Data.Linq
open FSharp.Data.TypeProviders

[<AutoOpen>]
module Types =
  type Post = {
    Id:int
    Title:string
    Content:string }

module DataAccess =
  type PostDto() = 
    member val Id = 0 with get, set
    member val Title = "" with get, set
    member val Content = "" with get, set

  [<Literal>]
  let private connectionString = 
    @"Data Source=.\SQLEXPRESS;Initial Catalog=MyBlog;User Id=myblog; Password=vboG&?0HYh"
  
  type SqlData = SqlDataConnection<connectionString>
  
  let private exec cs query =
    let ctx = SqlData.GetDataContext(cs)
    let mapper (dto:PostDto) = { Id = dto.Id; Title = dto.Title; Content = dto.Content }
    ctx.DataContext.ExecuteQuery<PostDto>(query) |> Seq.map mapper    

  let private all cs =
    exec cs "SELECT Id, CONVERT(VARCHAR(MAX), Title) AS Title, CONVERT(VARCHAR(MAX), Content) AS Content FROM Posts"

  let private find cs searchTerm =
    let escapeSingleQuotes (src:String) = src.Replace("'", "''") 
    searchTerm
    |> escapeSingleQuotes
    |> sprintf "SELECT Id, CONVERT(VARCHAR(MAX), Title) AS Title, CONVERT(VARCHAR(MAX), Content) AS Content FROM Posts WHERE FREETEXT(*,'%s')" 
    |> exec cs

  let getPosts cs (searchTerm:String) =
    if String.IsNullOrWhiteSpace(searchTerm) then
      all cs
    else
      find cs searchTerm

module PostSerializer =
  let listToJson =
    let toJson post = sprintf "{ \"Id\" : %d, \"Title\" : \"%s\", \"Content\" : \"%s\"}" post.Id post.Title post.Content
    Seq.map toJson
    >> String.concat ","
    >> sprintf "[%s]"

let app =
  let connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=MyBlog;User Id=myblog; Password=vboG&?0HYh"
  
  let jQuery = file "packages/jQuery/Content/Scripts/jquery-3.1.0.js"
  let js = file "packages/bootstrap/content/Scripts/bootstrap.min.js"
  let css = file "packages/bootstrap/content/Content/bootstrap.min.css"

  let search = 
    let getPosts searchTerm = 
      DataAccess.getPosts connectionString searchTerm 
      |> PostSerializer.listToJson  

    request (fun r ->
      match r.queryParam "searchterm" with
          | Choice1Of2 v -> v
          | _            -> ""
      |> fun searchTerm ->
        OK (getPosts searchTerm) >=> setMimeType "application/json; charset=utf-8")

  choose
    [ GET >=> choose 
        [ path "/" >=> file "web/index.html"; browseHome
          path "/api/posts" >=> search 
          path "/jquery.js" >=> Writers.setMimeType "text/javascript" >=> jQuery
          path "/bootstrap.min.js" >=> Writers.setMimeType "text/javascript" >=> js
          path "/bootstrap.min.css" >=> Writers.setMimeType "text/css" >=> css ]
      RequestErrors.NOT_FOUND "Found no handlers" ]      

startWebServer defaultConfig app