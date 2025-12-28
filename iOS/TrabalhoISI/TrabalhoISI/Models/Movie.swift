//
//  Movie.swift
//  TrabalhoISI
//
//  Created by Glória Martins on 26/12/2025.
//

import Foundation

struct Movie: Codable, Identifiable {
  var id: Int
  var title: String
  var originalTitle: String?
  var overview: String?
  var releaseDate: String?
  var posterPath: String?
  var backdropPath: String?
}


struct MoviesResponse: Codable {
  let total: Int
  let page: Int
  let pageSize: Int
  let totalPages: Int
  let data: [Movie]
}
