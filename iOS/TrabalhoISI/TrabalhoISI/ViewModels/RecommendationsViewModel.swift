//
//  RecommendationsViewModel.swift
//  TrabalhoISI
//
//  Created by Glória Martins on 27/12/2025.
//

import Foundation
import SwiftUI
import Combine

enum RecommendationType: String, CaseIterable {
  case moviesForBook
  case booksForMovie
  
  var title: String {
    switch self {
    case .moviesForBook: return "Movies for Book"
    case .booksForMovie: return "Books for Movie"
    }
  }
}

@MainActor
class RecommendationsViewModel: ObservableObject {
  
  @Published var selectedType: RecommendationType = .moviesForBook
  @Published var sourceId: String = ""
  
  @Published var recommendations: [RecommendationItem] = []
  @Published var isLoading = false
  @Published var errorMessage: String?
  
  func fetchRecommendations() async {
    isLoading = true
    errorMessage = nil
    recommendations = []
    
    do {
      switch selectedType {
        
      case .moviesForBook:
        guard let bookId = Int(sourceId) else {
          errorMessage = "Book ID must be a number"
          isLoading = false
          return
        }
        
        let result = try await APIService.shared.getMoviesForBook(bookId: bookId)
        recommendations = result.recommendations ?? []
        
      case .booksForMovie:
        guard let movieId = Int(sourceId) else {
          errorMessage = "Movie ID must be a number"
          isLoading = false
          return
        }
        
        let result = try await APIService.shared.getBooksForMovie(movieId: movieId)
        recommendations = result.recommendations ?? []
      }
      
    } catch {
      errorMessage = "Failed to fetch recommendations"
    }
    
    isLoading = false
  }
}
