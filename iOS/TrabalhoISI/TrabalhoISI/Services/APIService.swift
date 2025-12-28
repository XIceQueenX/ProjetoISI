//
//  APIService.swift
//  TrabalhoISI
//
//  Created by Glória Martins on 26/12/2025.
//

import Foundation

class APIService {
  static let shared = APIService()
  
  private let baseURL = "https://trabalho-isi-gdf5d0gnh4bwawfw.westeurope-01.azurewebsites.net//api"
  
  private init() {}
  
  // MARK: - Generic Request
  func request<T: Decodable>(
    endpoint: String,
    method: String = "GET",
    body: Data? = nil,
    requiresAuth: Bool = false
  ) async throws -> T {
    guard let url = URL(string: "\(baseURL)\(endpoint)") else {
      throw URLError(.badURL)
    }
    
    var request = URLRequest(url: url)
    request.httpMethod = method
    request.setValue("application/json", forHTTPHeaderField: "Content-Type")
    
    if requiresAuth, let token = KeychainHelper.shared.getToken() {
      request.setValue("Bearer \(token)", forHTTPHeaderField: "Authorization")
    }
    
    if let body = body {
      request.httpBody = body
    }
    
    let (data, response) = try await URLSession.shared.data(for: request)
    
    guard let httpResponse = response as? HTTPURLResponse else {
      throw URLError(.badServerResponse)
    }
    
    guard (200...299).contains(httpResponse.statusCode) else {
      throw URLError(.init(rawValue: httpResponse.statusCode))
    }
    
    let decoder = JSONDecoder()
    return try decoder.decode(T.self, from: data)
  }
  
  // MARK: - Books
  func getBooks(page: Int = 1, pageSize: Int = 20, search: String? = nil) async throws -> BooksResponse {
    var endpoint = "/Books?page=\(page)&pageSize=\(pageSize)"
    if let search = search, !search.isEmpty {
      endpoint += "&search=\(search.addingPercentEncoding(withAllowedCharacters: .urlQueryAllowed) ?? "")"
    }
    return try await request(endpoint: endpoint)
  }
  
  func getBook(id: Int) async throws -> Book {
    return try await request(endpoint: "/Books/\(id)")
  }
  
  func getBooksByAuthor(author: String) async throws -> [Book] {
    let encodedAuthor = author.addingPercentEncoding(withAllowedCharacters: .urlQueryAllowed) ?? ""
    return try await request(endpoint: "/Books/by-author/\(encodedAuthor)")
  }
  
  func createBook(book: Book) async throws -> Book {
    let encoder = JSONEncoder()
    let body = try encoder.encode(book)
    return try await request(endpoint: "/Books", method: "POST", body: body, requiresAuth: true)
  }
  
  func updateBook(id: Int, book: Book) async throws -> Book {
    let encoder = JSONEncoder()
    let body = try encoder.encode(book)
    return try await request(endpoint: "/Books/\(id)", method: "PUT", body: body, requiresAuth: true)
  }
  
  func deleteBook(id: Int) async throws {
    let _: [String: String] = try await request(endpoint: "/Books/\(id)", method: "DELETE", requiresAuth: true)
  }
  
  // MARK: - Movies
  func getMovies(page: Int = 1, pageSize: Int = 20, search: String? = nil) async throws -> MoviesResponse {
    var endpoint = "/Movies?page=\(page)&pageSize=\(pageSize)"
    if let search = search, !search.isEmpty {
      endpoint += "&search=\(search.addingPercentEncoding(withAllowedCharacters: .urlQueryAllowed) ?? "")"
    }
    return try await request(endpoint: endpoint)
  }
  
  func getMovie(id: Int) async throws -> Movie {
    return try await request(endpoint: "/Movies/\(id)")
  }
  
  func createMovie(movie: Movie) async throws -> Movie {
    let encoder = JSONEncoder()
    let body = try encoder.encode(movie)
    return try await request(endpoint: "/Movies", method: "POST", body: body, requiresAuth: true)
  }
  
  /// Update a movie completely (Admin only)
  func updateMovie(id: Int, movie: Movie) async throws -> Movie {
    let encoder = JSONEncoder()
    let body = try encoder.encode(movie)
    return try await request(endpoint: "/Movies/\(id)", method: "PUT", body: body, requiresAuth: true)
  }
  
  /// Partial update a movie (Admin only)
  func patchMovie(id: Int, partialData: [String: Any]) async throws -> Movie {
    let body = try JSONSerialization.data(withJSONObject: partialData, options: [])
    return try await request(endpoint: "/Movies/\(id)", method: "PATCH", body: body, requiresAuth: true)
  }
  
  /// Delete a movie (Admin only)
  func deleteMovie(id: Int) async throws {
    let _: [String: String] = try await request(endpoint: "/Movies/\(id)", method: "DELETE", requiresAuth: true)
  }
  
  // MARK: - Recommendations
  // MARK: - Recommendations
  func getMoviesForBook(bookId: Int) async throws -> RecommendationResult {
    return try await request(
      endpoint: "/Recommendation/movies-for-book/\(bookId)"
    )
  }
  
  func getBooksForMovie(movieId: Int) async throws -> RecommendationResult {
    return try await request(
      endpoint: "/Recommendation/books-for-movie/\(movieId)"
    )
  }
  
}
