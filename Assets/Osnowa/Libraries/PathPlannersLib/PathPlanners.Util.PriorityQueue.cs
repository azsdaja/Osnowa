namespace Libraries.PathPlannersLib
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class PriorityQueue<TKey, TValue>
    {
	    private static Stack<Queue<TValue>> _availableQueues = new Stack<Queue<TValue>>(100);
        private SortedList<TKey, Queue<TValue>> sortedList = new SortedList<TKey, Queue<TValue>>();

        public void Clear()
        {
            sortedList.Clear();
        }
        public bool IsEmpty()
        {
            return sortedList.Count == 0;
        }
        public void Enqueue(TKey key, TValue value)
        {
	        if (sortedList.ContainsKey(key) == false)
	        {
		        Queue<TValue> newQueue = FetchQueue();
				sortedList.Add(key, newQueue);
	        }

            sortedList[key].Enqueue(value);
        }

	    public TValue Dequeue()
        {
            if (sortedList.Count > 0)
            {
                KeyValuePair<TKey, Queue<TValue>> first = sortedList.First();
                TValue value = first.Value.Dequeue();
	            if (first.Value.Count == 0)
	            {
		            sortedList.Remove(first.Key);
					first.Value.Clear();
					_availableQueues.Push(first.Value);
	            }
                return value;
            }
            else throw new InvalidOperationException("Priority Queue is empty");
        }

	    private Queue<TValue> FetchQueue()
	    {
			if(_availableQueues.Count == 0)
			    return new Queue<TValue>();
			else
			{
				return _availableQueues.Pop();
			}
	    }
    }        
}
