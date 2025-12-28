//
//  BooksViewModel.swift
//  TrabalhoISI
//
//  Created by Glória Martins on 26/12/2025.
//

import Foundation
import Combine
import Foundation

@MainActor
class BooksViewModel: ObservableObject {
  @Published var books: [Book] = []
  @Published var searchText: String = ""
  @Published var isLoading = false
  private var currentPage = 1
  private var totalPages = 1
  var allBooks: [Book] = [] 
  
  func loadBooks() async {
    guard !isLoading else { return }
    isLoading = true
    
    do {
      let response = try await APIService.shared.getBooks(page: currentPage)
      books.append(contentsOf: response.data)
      allBooks.append(contentsOf: response.data)
      totalPages = response.totalPages
    } catch {
      print("Failed to load books:", error)
    }
    
    isLoading = false
  }
  
  func loadMoreIfNeeded(currentBook: Book) async {
    guard let last = books.last, last.id == currentBook.id else { return }
    guard currentPage < totalPages else { return }
    currentPage += 1
    await loadBooks()
  }
  
  func searchBooksByAuthor() async {
    if searchText.isEmpty {
      books = allBooks
      return
    }
    
    isLoading = true
    defer { isLoading = false }
    
    do {
      let results = try await APIService.shared.getBooksByAuthor(author: searchText)
      books = results
    } catch {
      print("Failed to search books:", error)
    }
  }
}
