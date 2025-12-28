//
//  MoviesViewModel.swift
//  TrabalhoISI
//
//  Created by Glória Martins on 26/12/2025.
//

import Foundation
import Combine
import Foundation
import Combine

@MainActor
class MoviesViewModel: ObservableObject {
  @Published var movies: [Movie] = []
  @Published var isLoading = false
  @Published var showAdminSheet = false
  @Published var selectedMovie: Movie? = nil
  @Published var isAdmin: Bool = false
  
  private var currentPage = 1
  private var totalPages = 1
  
  init() {
    checkAdminStatus()
  }
  
  private func checkAdminStatus() {
    if let roles = KeychainHelper.shared.getUserRole() {
      self.isAdmin = roles.contains("Admin")
    }
  }
  // MARK: - Load movies
  func loadMovies(page: Int = 1) async {
    guard !isLoading, page <= totalPages else { return }
    isLoading = true
    
    do {
      let response = try await APIService.shared.getMovies(page: page)
      if page == 1 {
        movies = response.data
      } else {
        movies.append(contentsOf: response.data)
      }
      currentPage = page
      totalPages = response.totalPages
    } catch {
      print("Failed to fetch movies:", error)
    }
    
    isLoading = false
  }
  
  func loadMoreIfNeeded(currentMovie movie: Movie) async {
    guard let last = movies.last, movie.id == last.id else { return }
    await loadMovies(page: currentPage + 1)
  }
  
  // MARK: - Admin operations
  func addOrEditMovie(movie: Movie) async {
    do {
      if let index = movies.firstIndex(where: { $0.id == movie.id }) {
        let updated = try await APIService.shared.updateMovie(id: movie.id, movie: movie)
        movies[index] = updated
      } else {
        let newMovie = try await APIService.shared.createMovie(movie: movie)
        movies.insert(newMovie, at: 0)
      }
    } catch {
      print("Failed to add/edit movie:", error)
    }
  }
  
  func deleteMovie(movie: Movie) async {
    do {
      try await APIService.shared.deleteMovie(id: movie.id)
      movies.removeAll { $0.id == movie.id }
    } catch {
      print("Failed to delete movie:", error)
    }
  }
}
