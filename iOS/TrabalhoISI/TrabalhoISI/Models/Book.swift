//
//  Book.swift
//  TrabalhoISI
//
//  Created by Glória Martins on 26/12/2025.
//

import Foundation

struct Book: Identifiable, Codable {
  let id: Int
  let title: String
  let subtitle: String?
  let authors: String
  let publisher: String?
  let publishedDate: String?
  let description: String?
  
  enum CodingKeys: String, CodingKey {
    case id, title, subtitle, authors, publisher
    case publishedDate = "publishedDate"
    case description
  }
}

struct BooksResponse: Codable {
  let total: Int
  let page: Int
  let pageSize: Int
  let totalPages: Int
  let data: [Book]
}
