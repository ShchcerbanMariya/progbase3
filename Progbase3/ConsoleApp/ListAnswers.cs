using System;
using ConsoleApp;

public class ListAnswers
{
    private Answer[] _items;
    private int _size;
    public ListAnswers()
    {
        _items = new Answer[16];
        _size = 0;
    }
    public void AddAnswer(Answer answer)
    {
        if (this._size >= this._items.Length)
            this.EnsureCapacity(_size);
        this._items[this._size] = answer;
        this._size++;
    }
    private void EnsureCapacity(int newSize)
    {
        if (newSize == _items.Length)
        {
            Array.Resize(ref _items, _items.Length * 2);
        }
    }
    public int GetSize()
    {
        return _size;
    }
    public Answer GetAnswer(int i)
    {
        if (i >= this._size)
            throw new Exception("Wrong  position");
        return _items[i];
    }
    public void RemoveAt(int index)
    {
        for (int i = index; i < GetSize(); i++)
        {
            _items[i] = _items[i + 1];
        }
        _size--;
    }
}
