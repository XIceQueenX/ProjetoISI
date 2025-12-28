//
//  MoviesView.swift
//  TrabalhoISI
//
//  Created by Glória Martins on 26/12/2025.
//


import SwiftUI

struct MoviesView: View {
  @StateObject var vm: MoviesViewModel = MoviesViewModel()
  @State private var showingAddSheet = false
  
  var body: some View {
    NavigationStack {
      List {
        ForEach(vm.movies) { movie in
          VStack(alignment: .leading, spacing: 4) {
            Text(movie.title)
              .font(.headline)
            if let original = movie.originalTitle {
              Text(original)
                .font(.subheadline)
                .foregroundColor(.gray)
                .lineLimit(2)
            }
            if let release = movie.releaseDate {
              Text(release)
                .font(.caption)
                .foregroundColor(.secondary)
            }
          }
          .padding(.vertical, 4)
          .onTapGesture {
            if vm.isAdmin {
              vm.selectedMovie = movie
            }
          }
          .task {
            await vm.loadMoreIfNeeded(currentMovie: movie)
          }
        }
        
        if vm.isLoading {
          HStack {
            Spacer()
            ProgressView()
            Spacer()
          }
        }
      }
      .navigationTitle("Movies")
      .toolbar {
        if vm.isAdmin {
          Button(action: {
            vm.selectedMovie = Movie(id: 0, title: "", originalTitle: nil, overview: nil, releaseDate: nil, posterPath: nil, backdropPath: nil)
          }) {
            Image(systemName: "plus")
          }
        }
      }
      .sheet(item: $vm.selectedMovie) { movie in
        AddOrEditMovieView(vm: vm, movie: movie)
      }
      .onAppear {
        Task { await vm.loadMovies() }
      }
    }
  }
}

#Preview {
  let vm = MoviesViewModel()
  vm.movies = [
    Movie(
      id: 1,
      title: "Inception",
      originalTitle: "Inception",
      overview: "A thief who steals corporate secrets through dream-sharing technology.",
      releaseDate: "2010-07-16",
      posterPath: nil,
      backdropPath: nil
    ),
    Movie(
      id: 2,
      title: "Interstellar",
      originalTitle: "Interstellar",
      overview: "A team of explorers travel through a wormhole in space.",
      releaseDate: "2014-11-07",
      posterPath: nil,
      backdropPath: nil
    )
  ]
  return MoviesView(vm: vm)
}
