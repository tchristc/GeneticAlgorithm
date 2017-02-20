using System.Collections.ObjectModel;

namespace GeneticAlgorithm
{
    public interface IGeneticAlgorithmParameters<T>
    {
        T Copy(T value);
        ReadOnlyCollection<T> Crossover(ReadOnlyCollection<Chromosome<T>> parents);
        double FitnessEvaluator(T chromosome);
        Population<T> GeneratePopulation();
        T Mutator(T chromosome);
        ReadOnlyCollection<Chromosome<T>> SelectFittestChildren(Population<T> population);
        Chromosome<T> Terminator(Population<T> population);

        int ChromosomeLength { get; }
        double CrossoverProbability { get; }
        double MutationProbability { get; }
        int NumberOfGenerations { get; }
        int NumberOfGenerationRuns { get; }
        int PopulationSize { get; }
        int TaskCount { get; }
        double WorstFitnessValue { get; }
    }
}
