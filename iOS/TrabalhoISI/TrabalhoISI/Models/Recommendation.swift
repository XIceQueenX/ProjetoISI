//
//  RecommendationResult.swift
//  TrabalhoISI
//
//  Created by Glória Martins on 26/12/2025.
//

import Foundation

struct RecommendationResult: Codable {
  let source: RecommendationSource
  let recommendations: [RecommendationItem]?
  let totalRecommendations: Int?
}


struct RecommendationSource: Codable {
  let id: Int?
  let title: String
  let authors: String?
  let releaseDate: String?
  
  enum CodingKeys: String, CodingKey {
    case id, title, authors, releaseDate
  }
}

struct RecommendationItem: Codable, Identifiable {
  let id = UUID()
  let title: String
  let overview: String?
  let description: String?
  let matchReason: String?
}

struct PersonalizedRecommendations: Codable {
  let movies: [Movie]
  let books: [Book]
}

struct PreferencesRequest: Codable {
  let genre: String?
  let keywords: [String]?
}
